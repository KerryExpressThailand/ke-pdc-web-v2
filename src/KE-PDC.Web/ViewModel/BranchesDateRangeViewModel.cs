using System;
using System.Collections.Generic;

namespace KE_PDC.ViewModel
{
    public class BranchesDateRangeViewModel
    {
        public int Page { get; set; }

        public int PerPage { get; set; }

        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }

        public List<string> BranchIdList { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Filter { get; set; }
    }
    public class ReqReconcile
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public List<BranchIdList> BranchIdList { get; set; }
       
    }

    public class ReqReconcileData
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string MatchStatus { get; set; }
        public List<BranchIdList> BranchIdList { get; set; }
       

    }

    public class BranchIdList
    {
        public string BranchId { get; set; }

    }

    public class DiscountTypeList
    {
        public string DiscountType { get; set; }

    }

    public class BranchesBillDateRangeViewModel
    {
        public int Page { get; set; }

        public int PerPage { get; set; }

        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }

        public List<string> BranchIdList { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Filter { get; set; }
    }

    public class BillDataViewModel
    {
        public int Page { get; set; }

        public int PerPage { get; set; }

        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }

        public List<string> BranchIdList { get; set; }
        public List<string> BranchType { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Filter { get; set; }
        public string MatchStatus { get; set; }
    }

    public class BranchMatching
    {
        public int Match { get; set; }

        public int Total { get; set; }
    }


    public class SumMatching
    {
        public List<Result> Result { get; set; }
    }
    public class Result
    {
        public string MatchPercent { get; set; }
        public string UnmatchPercent { get; set; }
        public string NoFoundPercent { get; set; }
        public string MatchBranch { get; set; }
        public string UnmatchBranch { get; set; }
        public string NoFoundBranch { get; set; }
    }


    public class ReconcileBillPaymentData
    {
        public List<ReconcileBillPayment> Result { get; set; }
    }

    public class ReconcileBillPayment
    {
        public string ID { get; set; }        
        public string ERP_ID { get; set; }        
        public string BranchID { get; set; }
        public string BranchType { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal EOD { get; set; }
        public decimal DailyRevenue { get; set; }
        public decimal? Transfer { get; set; }
        public int? ReconcileMatch { get; set; }
        public decimal? Variance { get; set; }
        public bool? CheckCloseShop { get; set; }
        public bool? IsAdjust { get; set; }

    }

    public class ConfirmListsModel
    {
        public List<ConfirmModel> ConfirmLists { get; set; }

    }

    public class ConfirmModel
    {
        public string BranchId { get; set; }
        public string ReportDate { get; set; }
        public string UserId { get; set; }
        public string VerifyDate { get; set; }
        public string RemittanceDate { get; set; }

    }
    public class AdjustListsModel
    {
        public AdjustBill AdjustLists { get; set; }
    }

    public class AdjustBill
    {
        public string BranchId { get; set; }
        public string UserId { get; set; }
        public string ReportDate { get; set; }
        public decimal BonusCommission { get; set; }
        public decimal AdjCreditCard { get; set; }
        public decimal AdjustmentOther { get; set; }
        public decimal ReturnCharge { get; set; }
        public decimal Suspense { get; set; }
        public decimal WithHoldingTax { get; set; }
        public decimal Promotion { get; set; }
        public decimal BankCharge { get; set; }
        public decimal AdjLinePay { get; set; }
        public decimal Project { get; set; }
        public string BonusCommissionRemark { get; set; }
        public string AdjCreditCardRemark { get; set; }
        public string AdjustmentOtherRemark { get; set; }
        public string ReturnChargeRemark { get; set; }
        public string SuspenseRemark { get; set; }
        public string WithHoldingTaxRemark { get; set; }
        public string PromotionRemark { get; set; }
        public string BankChargeRemark { get; set; }
        public string AdjLinePayRemark { get; set; }
        public string VerifyDate { get; set; }
        public string ProjectRemark { get; set; }
        public string RemittanceDate { get; set; }

    }

    public class BankChargeList
    {
        public List<BankCharge> AdjustLists { get; set; }
    }

    public class BankCharge
    {
        public string BranchId { get; set; }
        public string UserId { get; set; }
        public string ReportDate { get; set; }
        public decimal Variance { get; set; }

    }

    public class RollbackList
    {
        public List<Rollback> AdjustLists { get; set; }
    }

    public class Rollback
    {
        public string BranchId { get; set; }
        public string UserId { get; set; }
        public string ReportDate { get; set; }
        public decimal Variance { get; set; }

    }
}
