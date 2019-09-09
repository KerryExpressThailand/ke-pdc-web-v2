using System.ComponentModel.DataAnnotations;

namespace KE_PDC.ViewModel
{
    public class TypeBranchDateFromDateToViewModel
    {
        //[Required]
        [Display(Name = "Type")]
        public string BranchType { get; set; }

        //[Required]
        [Display(Name = "Branch")]
        public string BranchList { get; set; }

        public bool UseDateFrom { get; set; }
        public bool UseDateTo { get; set; }

        //[Required]
        [Display(Name = "Date From")]
        public string DateFrom { get; set; }

        //[Required]
        [Display(Name = "Date To")]
        public string DateTo { get; set; }

        [Display(Name = "Region")]
        public string Region { get; set; }

    }
}
