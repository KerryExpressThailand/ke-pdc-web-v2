using System;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class CloseShop
    {
        [Key]
        public string BranchID { get; set; }
        public string CostCenter { get; set; }
        public int EstCon { get; set; }
        public int EstBoxes { get; set; }
        public decimal EstRevenue { get; set; }
        public int Closed { get; set; }
        public DateTime? CloseDate { get; set; }
        public string CloseTime { get; set; }
        public string CloseBy { get; set; }
        public int Status { get; set; }
    }
}
