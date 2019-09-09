using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KE_PDC.ViewModel
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Old Password required")]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New Password required")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Confirmation Password required")]
        [CompareAttribute("Password", ErrorMessage = "Password doesn't match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmation Password")]
        public string ConfirmationPassword { get; set; }
    }
}
