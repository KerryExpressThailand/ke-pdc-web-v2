using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.ViewModel.ServicePoint
{
    public class ServicePointDaliyUpdateViewModel
    {
        [Required]
        [HiddenInput]
        public string ProfileId { get; set; }
        [Required]
        [HiddenInput]
        public string ReportDate { get; set; }
        [Required]
        public int Consignment { get; set; }
        [Required]
        public int Boxes { get; set; }
        [Required]
        public decimal Cash { get; set; }
    }
}
