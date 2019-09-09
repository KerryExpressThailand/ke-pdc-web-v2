using System.ComponentModel.DataAnnotations;

namespace KE_PDC.ViewModel
{
    public class MonthlyCommissionVerifyViewModel
    {
        [Required]
        public string BranchID { get; set; }
        [Required]
        public string MonthYear { get; set; }
    }
}
