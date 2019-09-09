using System.ComponentModel.DataAnnotations;

namespace KE_PDC.ViewModel.ServicePoint
{
    public class ServicePointDaliyVerifyViewModel
    {
        [Required]
        public string ProfileId { get; set; }
        [Required]
        public string ReportDate { get; set; }
    }
}
