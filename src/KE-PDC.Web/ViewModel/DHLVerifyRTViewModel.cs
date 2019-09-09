using System.ComponentModel.DataAnnotations;

namespace KE_PDC.ViewModel
{
    public class DHLVerifyRTUpdateViewModel
    {
        [Required]
        [Display(Name = "Branch")]
        public string BranchID { get; set; }

        [Required]
        [Display(Name = "Month/Year")]
        public string MonthYear { get; set; }

        //[Required]
        [Display(Name = "DHL Adjustment")]
        public string DHLAdjustment { get; set; }
    }
}
