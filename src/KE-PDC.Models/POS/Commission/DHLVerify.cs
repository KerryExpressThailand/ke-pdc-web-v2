using System;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class DHLVerify
    {
        [Key]
        public string BranchID { get; set; }
        public decimal DHLService { get; set; }
        public decimal DHLAdjustment { get; set; }
        public decimal DHLTotal { get; set; }
        public DateTime? FCConfirmDate { get; set; }
    }
}
