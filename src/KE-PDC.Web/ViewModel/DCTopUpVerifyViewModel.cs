using System.ComponentModel.DataAnnotations;

namespace KE_PDC.ViewModel
{
    public class DCTopUpVerifyViewModel
    {
        [Required]
        [Display(Name = "Report Date")]
        public string ReportDate { get; set; }

        [Required]
        [Display(Name = "Verify Date")]
        public string VerifyDate { get; set; }

        [Required]
        [Display(Name = "Remittance Date")]
        public string RemittanceDate { get; set; }

        [Display(Name = "Bonus Commission")]
        public decimal BonusCommission { get; set; }

        [Display(Name = "Adj Credit Card")]
        public decimal AdjustmentCreditCard { get; set; }

        [Display(Name = "Adjustment Other")]
        public decimal AdjustmentOther { get; set; }

        [Display(Name = "Return Charge")]
        public decimal ReturnCharge { get; set; }

        [Display(Name = "Suspensse")]
        public decimal Suspense { get; set; }

        [Display(Name = "Withholding Tax")]
        public decimal WithHoldingTax { get; set; }

        [Display(Name = "Promotion")]
        public decimal Promotion { get; set; }

        [Display(Name = "Bank Charge")]
        public decimal BankCharge { get; set; }

        [Display(Name = "Adj LinePay")]
        public decimal AdjustmentLinePay { get; set; }


        // Remark
        [Display(Name = "Bonus Commission Remark")]
        public string BonusCommissionRemark { get; set; }

        [Display(Name = "Adj CreditCard Remark")]
        public string AdjustmentCreditCardRemark { get; set; }

        [Display(Name = "Adjustment Other Remark")]
        public string AdjustmentOtherRemark { get; set; }

        [Display(Name = "Return Charge Remark")]
        public string ReturnChargeRemark { get; set; }

        [Display(Name = "Suspensse Remark")]
        public string SuspenseRemark { get; set; }

        [Display(Name = "Withholding Tax Remark")]
        public string WithHoldingTaxRemark { get; set; }

        [Display(Name = "Promotion Remark")]
        public string PromotionRemark { get; set; }

        [Display(Name = "Bank Charge Remark")]
        public string BankChargeRemark { get; set; }

        [Display(Name = "Adj LinePay Remark")]
        public string AdjustmentLinePayRemark { get; set; }
    }

    public class DCTopUpVerifySelectionViewModel
    {
        [Required]
        [Display(Name = "Branch List")]
        public string IDList { get; set; }

        [Required]
        [Display(Name = "Verify Date")]
        public string VerifyDate { get; set; }

        [Required]
        [Display(Name = "Remittance Date")]
        public string RemittanceDate { get; set; }
    }
}
