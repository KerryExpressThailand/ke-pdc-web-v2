using System;

namespace KE_PDC.Models.SevicePoint
{
    public class DailyCommission
    {
        public string ProfileId { get; set; }
        public DateTime ReportDate { get; set; }
        public string ProfileName { get; set; }
        public string BranchId { get; set; }
        public int Consignment { get; set; }
        public int Boxes { get; set; }
        public decimal Cash { get; set; }
        public decimal Commission { get; set; }
        public bool Verified { get; set; }
    }
}
