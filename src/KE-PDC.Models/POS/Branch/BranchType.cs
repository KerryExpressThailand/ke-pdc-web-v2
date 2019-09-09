using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class BranchType
    {
        [Key]
        public string TypeId { get; set; }
        public string TypeDescription { get; set; }
        public string TypeGroup { get; set; }
    }

    public class BranchTypes
    {
        [Key]
        public string TypeId { get; set; }
    }
}
