using System;

namespace KE_PDC.Models.POS.SevicePoint
{
    public class SevicePointDailyRevenue
    {
        public string BranchID { get; set; }
        public string ProfileID { get; set; }
        public DateTime ReportDate { get; set; }
        public string Type { get; set; }
        public int? Consignment { get; set; }
        public int? Boxes { get; set; }
        public decimal? Cash { get; set; }
        public decimal? LinePay { get; set; }
        public decimal? LineTopUpService { get; set; }
        public bool? Captured { get; set; }
        public DateTime? CapturedDate { get; set; }
        public string CapturedBy { get; set; }
        public bool? Approved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovedBy { get; set; }
        public bool? SendToERP { get; set; }
        public DateTime? SendToERPDate { get; set; }
    }
}
