using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CayGiaPhaTest
{
    public class Parents
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? FatherId { get; set; }
        public virtual User Father { get; set; }
        public int? MotherId { get; set; }
        public virtual User Mother { get; set; }
        public virtual HashSet<User> Children { get; set; }
    }
}
