using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models.POS
{
    public class MonthlyCommission
    {
        [Key]
        public string BranchID { get; set; }
        public string erp_id { get; set; }
        public int Boxes { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal Package { get; set; }
        public decimal SalesPackage { get; set; }
        public decimal CODSurcharge { get; set; }
        public decimal InsurSurcharge { get; set; }
        public decimal FreightRevenue { get; set; }
        public decimal DHL { get; set; }
        public decimal BSD { get; set; }
    }

    public class DailyCommission
    {
        [Key]
        public string ID { get; set; }
        public string BranchID { get; set; }
        public string erp_id { get; set; }
        public DateTime ReportDate { get; set; }
        public int Boxes { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal Package { get; set; }
        public decimal SalesPackage { get; set; }
        public decimal CODSurcharge { get; set; }
        public decimal InsurSurcharge { get; set; }
        public decimal FreightRevenue { get; set; }
        public decimal DHL { get; set; }
        public decimal BSD { get; set; }
    }

    public class MonthlySummaryCommission
    {
        public string ERPID { get; set; }
        [Key]
        public string BranchID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool Vatable { get; set; }

        // Rate
        public int? CommissionRate { get; set; }
        public int? DropOffRate { get; set; }
        public int? DHLRate { get; set; }
        public int? CODRate { get; set; }
        public int? InsureRate { get; set; }
        public int? BSDRate { get; set; }
        public int? RTSPRate { get; set; }
        public int? PSPRate { get; set; }

        public int? Boxes { get; set; }
        public decimal? PackageSurcharge { get; set; }
        public int? BoxMini { get; set; }
        public int? BoxS { get; set; }
        public int? BoxSPlus { get; set; }
        public int? BoxM { get; set; }
        public int? BoxMPlus { get; set; }
        public int? BoxL { get; set; }
        public decimal? Cash { get; set; }
        public decimal? Rabbit { get; set; }
        public decimal? CreditCard { get; set; }
        public decimal? LinePay { get; set; }
        public decimal? TotalRevenue { get; set; }
        public decimal? Transport { get; set; }
        public decimal? AM { get; set; }
        public decimal? PUP { get; set; }
        public decimal? SATDelivery { get; set; }
        public decimal? RAS { get; set; }
        public decimal? TotalFreightRevenue { get; set; }
        public decimal? DHLAmount { get; set; }
        public decimal? DHLAdjustment { get; set; }
        public decimal? CODAmount { get; set; }
        public decimal? InsuranceAmount { get; set; }
        public decimal? SamedayCITY { get; set; }
        public decimal? SamedayCITYN { get; set; }
        public decimal? SamedayCITYS { get; set; }
        public decimal? SamedayGRABEX { get; set; }
        public decimal? TotalSamedayRevenue { get; set; }
        public int? DropOffRevenue { get; set; }
        public decimal? IncomeTotalFreightRevenue { get; set; }
        public decimal? IncomeDHL { get; set; }
        public decimal? IncomeCOD { get; set; }
        public decimal? IncomeInsurance { get; set; }
        public decimal? IncomeSameday { get; set; }
        public int? IncomeDropoff { get; set; }
        public decimal? IncomeRTSP { get; set; }
        public decimal? IncomePSP { get; set; }
        public decimal? TotalIncome { get; set; }
        public decimal? ExpenseCOD { get; set; }
        public decimal? ExpenseInsurance { get; set; }
        public decimal? ExpenseFeeManagement { get; set; }
        public decimal? ExpenseFeeIT { get; set; }
        public decimal? ExpenseFeeSupply { get; set; }
        public decimal? ExpenseFeeFacility { get; set; }
        public decimal? ExpenseSalesPackage { get; set; }
        public decimal? TotalExpense { get; set; }
        public decimal? Adjustment { get; set; }
        public string AdjustmentRemark { get; set; }
        public decimal? TotalCommission { get; set; }
        public decimal? NetCommission { get; set; }
        public string FeeManagementVerifyBy { get; set; }
        public DateTime? FeeManagementVerifyDate { get; set; }
        public string FeeItVerifyBy { get; set; }
        public DateTime? FeeItVerifyDate { get; set; }
        public string FeeSupplyVerifyBy { get; set; }
        public DateTime? FeeSupplyVerifyDate { get; set; }
        public string FeeFacilityVerifyBy { get; set; }
        public DateTime? FeeFacilityVerifyDate { get; set; }
        public string SalesPackageVerifyBy { get; set; }
        public DateTime? SalesPackageVerifyDate { get; set; }
        public string FcConfirmBy { get; set; }
        public DateTime? FcConfirmDate { get; set; }
        public string PRNo { get; set; }
        public DateTime? PRDate { get; set; }
        public bool? SendToERP { get; set; }
        public DateTime? SendToERPDate { get; set; }
        public List<MonthlyExpenseDetail> MonthlyExpenseDetail { get; set; }
    }
}
