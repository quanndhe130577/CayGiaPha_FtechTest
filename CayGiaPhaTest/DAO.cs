using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CayGiaPhaTest
{
    public class DAO
    {
        public enum Gender
        {
            Male,
            Female,
            Both
        }

        public enum Relation
        {
            Father,
            Mother,
            Both
        }

        public static async Task<User> GetUserByIdASync(int id)
        {
            using (var _context = new GPDbContext())
            {
                return await _context.Users.FindAsync(id);
            }
        }

        /// <summary>
        /// Hàm lấy ra bố hoặc mẹ hoặc cả hai
        /// </summary>
        /// <param name="child">User cần tìm thông tin</param>
        /// <param name="gender">True là bố, false là mẹ</param>
        /// <returns>Bố hoặc mẹ</returns>
        public static async Task<List<User>> GetParentAsync(User child, Gender gender = Gender.Both)
        {
            using (var _context = new GPDbContext())
            {
                var parents = await _context.Parents.FindAsync(child.ParentsId);
                switch (gender)
                {
                    case Gender.Male:
                        {
                            return _context.Users.Where(x => x.ID == parents.FatherId).ToList();
                        }
                    case Gender.Female:
                        {
                            return _context.Users.Where(x => parents.MotherId == x.ID).ToList();
                        }
                    default:
                        {
                            return _context.Users.Where(x => x.ID == parents.FatherId || parents.MotherId == x.ID).ToList();
                        }
                }
            }
        }

        /// <summary>
        /// Hàm tìm ra tất cả cháu của user theo cấc bậc
        /// </summary>
        /// <param name="user">Người cần lấy thông tin</param>
        /// <param name="level">Đời thứ n cần tìm, ví dụ : 1 là đời con, 2 là đời cháu, 3 là đời chắt, ....</param>
        /// <param name="gender">Giới tính của cháu cần tìm</param>
        /// <returns>List các user là cháu đời thứ n của User</returns>
        public static List<User> GetGrandChildrenByLevel(User user, int level = 2, Gender gender = Gender.Both)
        {
            using (var _context = new GPDbContext())
            {
                List<User> list = new List<User>();
                // lấy ra tất cả cặp vợ ck của user
                var listParentId = _context.Parents.Where(x => x.FatherId == user.ID || x.MotherId == user.ID).Select(x => x.Id).ToList();

                if (level == 1)
                {
                    list.AddRange(GetChildren(user, gender));
                }
                else if (level > 1)
                {
                    // lấy ra tất cả con
                    var children = GetChildren(user, Gender.Both);
                    foreach (var item in children)
                    {
                        list.AddRange(GetGrandChildrenByLevel(item, level - 1, gender));
                    }

                }

                return list;
            }
        }

        /// <summary>
        /// Hàm lấy ra tất cả cáu cháu theo cấc bấc, và theo quan hệ
        /// </summary>
        /// <param name="user">Người cần tìm thông tin</param>
        /// <param name="level">Đời thứ n cần tìm</param>
        /// <param name="gender">Giới tính của cháu cần tìm</param>
        /// <param name="relation">Tìm cháu nội hay cháu ngoại hay cả hai</param>
        /// <returns></returns>
        public static List<User> GetGrandChildrenByLevelAndRelation(User user, int level = 2, Gender gender = Gender.Both, Relation relation = Relation.Both)
        {
            if (relation == Relation.Both)
            {
                return GetGrandChildrenByLevel(user, level, gender);
            }
            using (var _context = new GPDbContext())
            {
                // lấy ra tất cả cặp vợ chồng của user
                var listParentId = _context.Parents.Where(x => x.FatherId == user.ID || x.MotherId == user.ID).Select(x => x.Id).ToList();
                var listChildren = new List<User>();

                if (relation == Relation.Father) // cháu nội
                {
                    // tìm tất cả con trai
                    listChildren.AddRange(GetChildren(user, Gender.Male));
                }
                else // cháu ngoại
                {
                    // tìm tất cả con gái
                    listChildren.AddRange(GetChildren(user, Gender.Female));
                }

                // tìm tất cả cháu đời tiếp theo
                var list = new List<User>();
                foreach (var item in listChildren)
                {
                    list.AddRange(GetGrandChildrenByLevel(item, level - 1, gender));
                }

                return list;
            }
        }

        /// <summary>
        /// Tìm tất cả con của user
        /// </summary>
        /// <param name="user">Người cần tìm thông tin</param>
        /// <param name="gender">Giới tính của con</param>
        /// <returns>List user</returns>
        public static List<User> GetChildren(User user, Gender gender = Gender.Both)
        {
            using (var _context = new GPDbContext())
            {
                // lấy ra tất cả cặp vợ chồng của user
                var listParentId = _context.Parents.Where(x => x.FatherId == user.ID || x.MotherId == user.ID).Select(x => x.Id).ToList();

                switch (gender)
                {
                    case Gender.Male:
                        {
                            // con trai
                            return _context.Users.Where(x => x.Gender && listParentId.Contains(x.ParentsId)).ToList();
                        }
                    case Gender.Female:
                        {
                            // con gái
                            return _context.Users.Where(x => !x.Gender && listParentId.Contains(x.ParentsId)).ToList();
                        }
                    default:
                        {
                            // cả hai
                            return _context.Users.Where(x => listParentId.Contains(x.ParentsId)).ToList();
                        }
                }
            }
        }

        private static async Task<Parents> GetParentAsync(User child)
        {
            using (var _context = new GPDbContext())
            {
                return await _context.Parents.FindAsync(child.ParentsId);
            }
        }

        /// <summary>
        /// Tìm tất cả trưởng bối theo đời
        /// </summary>
        /// <param name="user">Người cần tìm thông tin</param>
        /// <param name="level">Đời thứ n</param>
        /// <param name="gender">Giới tình của trưởng bối</param>
        /// <returns>List user</returns>
        public static async Task<List<User>> GetGrandByLevelAsync(User user, int level = 2, Gender gender = Gender.Both)
        {
            using (var _context = new GPDbContext())
            {
                List<User> list = new List<User>();
                var parents = await GetParentAsync(user);
                if (level == 1)
                {
                    switch (gender)
                    {
                        case Gender.Male:
                            {
                                // bố
                                list.Add(await _context.Users.FindAsync(parents.FatherId));
                                break;
                            }
                        case Gender.Female:
                            {
                                // mẹ
                                list.Add(await _context.Users.FindAsync(parents.MotherId));
                                break;
                            }
                        default:
                            {
                                // cả bố và mẹ
                                list.AddRange(_context.Users.Where(x => x.ID == parents.FatherId || x.ID == parents.MotherId));
                                break;
                            }
                    }
                }
                else
                {
                    // Nếu có bố
                    var fa = await _context.Users.FindAsync(parents.FatherId);
                    if (fa != null)
                    {
                        var rs_fa = await GetGrandByLevelAsync(fa, level - 1, gender);
                        list.AddRange((IEnumerable<User>)rs_fa);
                    }

                    // nếu có mẹ
                    var mo = await _context.Users.FindAsync(parents.MotherId);
                    if (mo != null)
                    {
                        var rs_mo = await GetGrandByLevelAsync(mo, level - 1, gender);
                        list.AddRange((IEnumerable<User>)rs_mo);
                    }

                }

                return list;
            }

        }

        /// <summary>
        /// Tìm tất cả trưởng bối theo đời và theo quan hệ
        /// </summary>
        /// <param name="user">Người cần tìm thông tin</param>
        /// <param name="level">Đời thứ n</param>
        /// <param name="gender">Giới tính của trưởng bối</param>
        /// <param name="relation">Bên nội hay bên ngoại hay cả hai</param>
        /// <returns>List User</returns>
        public static async Task<List<User>> GetGrandByLevelAndRelationAsync(User user, int level = 2, Gender gender = Gender.Both, Relation relation = Relation.Both)
        {
            if (level == 1) // Bố, mẹ
            {
                return await GetParentAsync(user, gender);
            }
            else if (level == 0)
            {
                return new List<User>();
            }

            if (relation == Relation.Both)
            {
                return await GetGrandByLevelAsync(user, level, gender);
            }

            using (var _context = new GPDbContext())
            {
                var parent = new List<User>();
                if (relation == Relation.Father) // họ nội
                {
                    parent = await GetParentAsync(user, Gender.Male);
                }
                else // họ ngoại
                {
                    parent = await GetParentAsync(user, Gender.Female);
                }

                return await GetGrandByLevelAsync(parent[0], level - 1, gender);
            }
        }

        /// <summary>
        /// Tìm anh em họ theo đời và giới tính
        /// </summary>
        /// <param name="user">Người cần tìm thông tin</param>
        /// <param name="level">Đời thứ n</param>
        /// <param name="gender">Giới tính</param>
        /// <returns>List user</returns>
        public static async Task<List<User>> GetCousinByLevelAsync(User user, int level, Gender gender)
        {
            var grand = await GetGrandByLevelAsync(user, level + 1, Gender.Both);
            var cousin = GetGrandChildrenByLevel(grand[0], level + 1, gender);
            cousin.Remove(user); // loại đi chính user nếu có
            return cousin;
        }

        /// <summary>
        /// Tìm ra tất cả vợ/chồng
        /// </summary>
        /// <param name="user">Người cần tìm</param>
        /// <returns>List user</returns>
        public static List<User> GetCouple(User user)
        {
            using (var _context = new GPDbContext())
            {
                var listId = new List<int?>();
                switch (user.Gender)
                {
                    case true:
                        {
                            listId = _context.Parents.Where(x => x.FatherId == x.Id).Select(x => x.MotherId).ToList();
                            break;
                        }
                    case false:
                        {
                            listId = _context.Parents.Where(x => x.MotherId == x.Id).Select(x => x.FatherId).ToList();
                            break;
                        }
                }

                return _context.Users.Where(x => listId.Contains(x.ID)).ToList();
            }
        }

        /// <summary>
        /// Tìm anh em cọc chèo
        /// </summary>
        /// <param name="user">Người cần tìm</param>
        /// <returns></returns>
        public static List<User> TimAnhEmCocCheo(User user)
        {
            if (!user.Gender)
            {
                throw new Exception("User must be Male");
            }

            var listWife = GetCouple(user);
            List<User> listAnhEmCocCheo = new List<User>();

            using (var _context = new GPDbContext())
            {
                foreach (var item in listWife)
                {
                    // tất cả chị em của vợ
                    var listChiEm = _context.Users.Where(x => !x.Gender && x.ParentsId == item.ParentsId && x.ID != item.ID);
                    foreach (var subitem in listChiEm)
                    {
                        // tất cả chồng của chị em của vợ
                        listAnhEmCocCheo.AddRange(GetCouple(subitem));
                    }
                }
            }

            return listAnhEmCocCheo;
        }
    }
}
