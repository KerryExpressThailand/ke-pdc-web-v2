using System;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models.POS
{
    public class ReconcileAdjustMaster
    {
        [Key]
        public string AdjustId { get; set; }
        public string AdjustDescription { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
