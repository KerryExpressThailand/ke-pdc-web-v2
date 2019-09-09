using System.ComponentModel.DataAnnotations;

namespace KE_PDC.ViewModel.ServicePoint
{
    public class ServicePointDaliyGetViewModel
    {
        [Required]
        public string ProfileIds { get; set; }
        [Required]
        public string DateFrom { get; set; }
        [Required]
        public string DateTo { get; set; }
        public string Status { get; set; }
        public bool? Excel { get; set; }
    }
}
