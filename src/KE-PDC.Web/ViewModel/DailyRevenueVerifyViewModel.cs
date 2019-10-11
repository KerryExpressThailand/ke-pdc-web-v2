using System;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.ViewModel
{
    public class DailyRevenueDetailViewModel
    {
        [Required]
        public string Branch { get; set; }

        [Required]
        public DateTime ReportDate { get; set; }

        //[Required]
        [Display(Name = "Comm")]
        public decimal Comm { get; set; }

        [Required]
        [Display(Name = "Adj Credit Card")]
        public decimal AdjCreditCard { get; set; }

        [Required]
        [Display(Name = "Other")]
        public decimal Other { get; set; }

        [Required]
        [Display(Name = "Return")]
        public decimal Return { get; set; }

        [Required]
        [Display(Name = "Suspensse")]
        public decimal Suspensse { get; set; }

        [Required]
        [Display(Name = "Withholding Tax")]
        public decimal WithHoldingTax { get; set; }

        [Required]
        [Display(Name = "Promotion")]
        public decimal Promotion { get; set; }

        [Required]
        [Display(Name = "Bank Charge")]
        public decimal BankCharge { get; set; }

        [Required]
        [Display(Name = "Adj LinePay")]
        public decimal AdjLinePay { get; set; }

        [Required]
        [Display(Name = "Verify Date")]
        public string VerifyDate { get; set; }


        //add by kathawutpa 17/7/2019 for project sales x
        [Required]
        [Display(Name = "Project")]
        public decimal Project { get; set; }

        [Required]
        [Display(Name = "Remittance Date")]
        public string RemittanceDate { get; set; }

        // Remark
        [Display(Name = "Comm Remark")]
        public string CommRemark { get; set; }

        [Display(Name = "Adj Credit Card Remark")]
        public string AdjCreditCardRemark { get; set; }

        [Display(Name = "Other Remark")]
        public string OtherRemark { get; set; }

        [Display(Name = "Return Remark")]
        public string ReturnRemark { get; set; }

        [Display(Name = "Suspensse Remark")]
        public string SuspensseRemark { get; set; }

        [Display(Name = "Withholding Tax Remark")]
        public string WithHoldingTaxRemark { get; set; }

        [Display(Name = "Pick-up Charge Remark")]
        public string PromotionRemark { get; set; }

        [Display(Name = "Bank Charge Remark")]
        public string BankChargeRemark { get; set; }

        [Display(Name = "Adj LinePay Remark")]
        public string AdjLinePayRemark { get; set; }

        //add by kathawutpa 17/7/2019 for project sales x
        [Display(Name = "Project Remark")]
        public string ProjectRemark { get; set; }

        [Display(Name = "Check Reconcile")]
        public string CheckReconcile { get; set; }

        [Display(Name = "Check mPayService")]
        public string mPayService { get; set; }

        [Display(Name = "Rabbit Top-Up")]
        public string rabbitTopUp { get; set; }
    }
}
