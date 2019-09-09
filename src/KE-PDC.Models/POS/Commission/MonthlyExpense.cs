using System;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class MonthlyExpense
    {
        [Key]
        public string ERPID { get; set; }
        public string BranchID { get; set; }
        public bool Vatable { get; set; }
        public decimal? ManagementFee { get; set; }
        public decimal? ServiceFeeIT { get; set; }
        public decimal? ServiceFeeSupply { get; set; }
        public decimal? Facility { get; set; }
        public decimal? FacilityVat { get; set; }
        public decimal? SalesPackage { get; set; }
        public decimal? Adjustment { get; set; }
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
    }

    public class MonthlyExpenseDetail
    {
        public string BranchID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int ItemID { get; set; }
        public string ItemDesc { get; set; }
        public int CategoryID { get; set; }
        public int? ItemAmount { get; set; }
        public decimal? ItemExpense { get; set; }
        public string Remark { get; set; }
        public string Attachment { get; set; }
    }
}
