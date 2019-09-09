using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.ViewModel
{
    public class BranchMonthlyViewModel
    {
        [Required]
        [Display(Name = "Branch")]
        public string BranchList { get; set; }
        public List<string> BranchIdList { get; set; }

        [Required]
        [Display(Name = "Year/Month")]
        public string MonthYear { get; set; }
    }
}
