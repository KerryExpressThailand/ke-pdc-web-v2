using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class BranchTypeGroup
    {
        [Key]
        public string TypeGroupId { get; set; }
        public string TypeGroupName { get; set; }
    }
}
