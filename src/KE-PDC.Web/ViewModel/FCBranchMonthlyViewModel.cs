using System.ComponentModel.DataAnnotations;

namespace KE_PDC.ViewModel
{
    public class FCBranchMonthlyViewModel
    {
        //[Required]
        [Display(Name = "FC")]
        public string FCGroup { get; set; }

        //[Required]
        [Display(Name = "Branch")]
        public string BranchList { get; set; }

        [Required]
        [Display(Name = "Year/Month")]
        public string MonthYear { get; set; }
    }
}
