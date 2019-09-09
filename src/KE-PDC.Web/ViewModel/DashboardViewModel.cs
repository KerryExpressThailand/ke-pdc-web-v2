using System.ComponentModel.DataAnnotations;

namespace KE_PDC.ViewModel
{
    public class CloseShopViewModel
    {
        [Required]
        [Display(Name = "Type")]
        public string BranchType { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public string BranchList { get; set; }

        [Required]
        [Display(Name = "Date From")]
        public string DateFrom { get; set; }

        public string FileType { get; set; }
    }

    public class ShopDailyViewModel
    {
        [Required]
        [Display(Name = "Branch")]
        public string BranchList { get; set; }

        [Required]
        [Display(Name = "Date")]
        public string Date { get; set; }
    }
}
