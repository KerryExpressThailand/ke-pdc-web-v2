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

    public class ResultDiscount
    {
        [Key]
        public List<Discount> Result { get; set; }
    }

    public class Discount
    {
        [Key]
        public string Discount_Code { get; set; }
        public string Discount_Description { get; set; }
    }
}
