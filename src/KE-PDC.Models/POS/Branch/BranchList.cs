using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class BranchList
    {
        [Key]
        public string bid { get; set; }
        public string bName { get; set; }
    }

    public class BranchList_r
    {
        [Key]
        public string bid { get; set; }
        public string bName { get; set; }
        public string rID { get; set; }
        public string rName { get; set; }
    }
}
