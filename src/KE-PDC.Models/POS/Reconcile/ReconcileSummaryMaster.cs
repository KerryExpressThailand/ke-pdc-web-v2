using System;

namespace KE_PDC.Models.POS {
    public class ReconcileSummaryMaster
    {
        public string BranchId { get; set; }
        public DateTime ReportDate { get; set; }
        public string TypeId { get; set; }
        public decimal Amount { get; set; }
        public decimal? Commission { get; set; }
        public decimal? Tax { get; set; }
        public bool? VerifiedFlag { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public string VerifiedBy { get; set; }
        public bool? ConfirmFlag { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public string ConfirmBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
