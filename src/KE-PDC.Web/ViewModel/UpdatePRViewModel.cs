using System.ComponentModel.DataAnnotations;

namespace KE_PDC.ViewModel
{
    public class UpdatePRViewModel
    {
        [Required]
        public string MonthYear { get; set; }
        [Required]
        public string[] BranchID { get; set; }
        [Required]
        public string[] PR { get; set; }
    }
}
