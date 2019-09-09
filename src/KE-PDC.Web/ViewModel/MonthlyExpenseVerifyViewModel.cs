using System.ComponentModel.DataAnnotations;

namespace KE_PDC.ViewModel
{
    public class MonthlyExpenseVerifyViewModel
    {
        [Required]
        public string BranchID { get; set; }
        [Required]
        public string MonthYear { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
