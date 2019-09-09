using System;

namespace KE_PDC.Models.POS
{
    public class DailyRevenueReconcileBillPayment
    {
        public string ERPID { get; set; }
        public string BranchID { get; set; }
        public string BranchType { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal EOD { get; set; }
        public decimal DailyRevenue { get; set; }
        public decimal? Tranfer { get; set; }
        public bool VerifyEOD { get; set; }
        public bool VerifyTranfer { get; set; }
        public decimal? Variance { get; set; }
    }

   

    public class DailyRevenueReconcileCards
    {
        public string ERPID { get; set; }
        public string BranchID { get; set; }
        public string BranchType { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal EODCreditBBL { get; set; }
        public decimal EODCreditSCB { get; set; }
        public decimal EODRabbit { get; set; }
        public decimal EOD { get; set; }
        public decimal DailyRevenueCreditBBL { get; set; }
        public decimal DailyRevenueCreditSCB { get; set; }
        public decimal DailyRevenueRabbit { get; set; }
        public decimal DailyRevenue { get; set; }
        public decimal? Tranfer { get; set; }
        public bool VerifyEODCreditBBL { get; set; }
        public bool VerifyEODCreditSCB { get; set; }
        public bool VerifyEODRabbit { get; set; }
        public bool VerifyEOD { get; set; }
        public bool VerifyTranfer { get; set; }
        public decimal? Variance { get; set; }
    }

    public class DailyRevenueReconcileLinePay
    {
        public string ERPID { get; set; }
        public string BranchID { get; set; }
        public string BranchType { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal EOD { get; set; }
        public decimal DailyRevenue { get; set; }
        public decimal? Tranfer { get; set; }
        public bool VerifyEOD { get; set; }
        public bool VerifyTranfer { get; set; }
        public decimal? Variance { get; set; }
    }

    public class DailyRevenueReconcileQrPayment
    {
        public string ERPID { get; set; }
        public string BranchID { get; set; }
        public string BranchType { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal EOD { get; set; }
        public decimal DailyRevenue { get; set; }
        public decimal? Tranfer { get; set; }
        public bool VerifyEOD { get; set; }
        public bool VerifyTranfer { get; set; }
        public decimal? Variance { get; set; }
    }
}
