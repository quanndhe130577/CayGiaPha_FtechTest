using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CayGiaPhaTest
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public int ParentsId { get; set; }
        public virtual Parents Parents { get; set; }

        public virtual HashSet<Parents> Fathers { get; set; }
        public virtual HashSet<Parents> Mothers { get; set; }

    }
}
