using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class SaveDBResponse
    {
        [Key]
        public string code { get; set; }
        public string desc { get; set; }
    }
}
