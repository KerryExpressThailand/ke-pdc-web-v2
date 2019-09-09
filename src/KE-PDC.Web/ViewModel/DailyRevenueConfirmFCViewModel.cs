using System.ComponentModel.DataAnnotations;

namespace KE_PDC.ViewModel
{
    public class DailyRevenueConfirmFCViewModel
    {
        [Required]
        [Display(Name = "Confirm by")]
        public string ConfirmBy { get; set; }

        //[Required]
        [Display(Name = "Branch")]
        public string BranchList { get; set; }

        [Required]
        [Display(Name = "Date From")]
        public string DateFrom { get; set; }

        //[Required]
        [Display(Name = "Date To")]
        public string DateTo { get; set; }
    }
}
