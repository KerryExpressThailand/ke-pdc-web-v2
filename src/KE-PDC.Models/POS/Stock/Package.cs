using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class Package
    {
        [Key]
        public string PackageID { get; set; }
        public string PackageDesc { get; set; }
        public int? PackageType { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
