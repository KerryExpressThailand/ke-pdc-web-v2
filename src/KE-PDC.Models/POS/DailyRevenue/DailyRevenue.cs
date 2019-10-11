using System;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models.POS
{
    public class DailyRevenue {
        public string BranchId { get; set; }
        public DateTime ReportDate { get; set; }
        public int? Consignment { get; set; }
        public int? Boxes { get; set; }
        public int? Sender { get; set; }
        public int? MonthlySender { get; set; }
        public int? AmCon { get; set; }
        public int? AmBoxes { get; set; }
        public decimal? Cash { get; set; }
        public decimal? Rabbit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? CreditBbl { get; set; }
        public decimal? CreditScb { get; set; }
        public decimal? QrPayment { get; set; }
        public decimal? LinePay { get; set; }
        public decimal? FreightSurcharge { get; set; }
        public decimal? AmSurcharge { get; set; }
        public decimal? PupSurcharge { get; set; }
        public decimal? SatDelSurcharge { get; set; }
        public decimal? RemoteAreaSurcharge { get; set; }
        public decimal? CodSurcharge { get; set; }
        public decimal? CodAmount { get; set; }
        public int? CodTotalCon { get; set; }
        public decimal? InsurSurcharge { get; set; }
        public decimal? PkgSurcharge { get; set; }
        public decimal? VatSurcharge { get; set; }
        public decimal? VatInsurSurcharge { get; set; }
        public decimal? VatCodSurcharge { get; set; }
        public decimal? VatPkgSurcharge { get; set; }
        public decimal? DiscountSurcharge { get; set; }
        public decimal? PkgService { get; set; }
        public decimal? DhlService { get; set; }
        public decimal? LineTopUpService { get; set; }
        public decimal? VatService { get; set; }
        public decimal? VatPkgService { get; set; }
        public decimal? CashForService { get; set; }
        public decimal? RabbitForService { get; set; }
        public decimal? CreditForService { get; set; }
        public decimal? LinePayForService { get; set; }
        public decimal? BsdForService { get; set; }
        public decimal? BsdSurcharge { get; set; }
        public decimal? CitySurcharge { get; set; }
        public decimal? CitynSurcharge { get; set; }
        public decimal? CitysSurcharge { get; set; }
        public decimal? CityvSurcharge { get; set; }
        public decimal? GrabexSurcharge { get; set; }
        public decimal? BsdDiscount { get; set; }
        public decimal? CityDiscount { get; set; }
        public decimal? CitynDiscount { get; set; }
        public decimal? CitysDiscount { get; set; }
        public decimal? CityvDiscount { get; set; }
        public decimal? GrabexDiscount { get; set; }
        public decimal? BsdLinePay { get; set; }
        public decimal? BsdCash { get; set; }
        public decimal? BsdLineTopUp { get; set; }
        public int? BsdCon { get; set; }
        public int? BsdBoxes { get; set; }
        public decimal? BsdcodSurcharge { get; set; }
        public decimal? BsdcodAmount { get; set; }
        public int? BsdcodTotalCon { get; set; }
        public int? DropOffBox { get; set; }
        public decimal? TudForService { get; set; }
        public string TudVerifyBy { get; set; }
        public DateTime? TudVerifyDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Captured { get; set; }
        public DateTime? CapturedDate { get; set; }
        public string CapturedBy { get; set; }
        public decimal? BonusCommission { get; set; }
        public string BonusCommissionRemark { get; set; }
        public decimal? AdjOther { get; set; }
        public string AdjOtherRemark { get; set; }
        public decimal? ReturnCharge { get; set; }
        public string ReturnChargeRemark { get; set; }
        public decimal? Suspense { get; set; }
        public string SuspenseRemark { get; set; }
        public decimal? WithHoldingTax { get; set; }
        public string WithHoldingTaxRemark { get; set; }
        public decimal? Promotion { get; set; }
        public string PromotionRemark { get; set; }

        //add by kathawutpa 17/7/2019 for project sales x
        public decimal? Project { get; set; }
        public string ProjectRemark { get; set; }

        public decimal? BankCharge { get; set; }
        public string BankChargeRemark { get; set; }
        public decimal? AdjCreditCardCharge { get; set; }
        public string AdjCreditCardRemark { get; set; }
        public decimal? AdjLinePayCharge { get; set; }
        public string AdjLinePayRemark { get; set; }
        public DateTime? VerifyDate { get; set; }
        public DateTime? RemittanceDate { get; set; }
        public bool? SendToKes { get; set; }
        public DateTime? SendToKesDate { get; set; }
        public bool? SendToErp { get; set; }
        public DateTime? SendToErpDate { get; set; }
        public bool? BsdProcessed { get; set; }
        public DateTime? BsdProcessedDate { get; set; }
        public bool? Approved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovedBy { get; set; }
        public bool? FcConfirmed { get; set; }
        public DateTime? FcConfirmedDate { get; set; }
        public string FcConfirmedBy { get; set; }
        public DateTime? PsaSyncDate { get; set; }
        public DateTime? PdcSyncDate { get; set; }
        public DateTime? ErpSyncDate { get; set; }
        public DateTime? RejectDate { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class DailyRevenueVerify
    {
        [Key]
        public string ID { get; set; }
        public string ERP_ID { get; set; }
        public string ShopType { get; set; }
        public string BranchID { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal Freight { get; set; }
        public decimal Transport { get; set; }
        public decimal AM { get; set; }
        public decimal PUP { get; set; }
        public decimal SAT { get; set; }
        public decimal RAS { get; set; }
        public decimal COD { get; set; }
        public decimal Insur { get; set; }
        public decimal Pkg { get; set; }
        public decimal SalePackage { get; set; }
        public decimal LineTopUp { get; set; }
        public decimal Cash { get; set; }
        public decimal Rabbit { get; set; }
        public decimal CreditBBL { get; set; }
        public decimal CreditSCB { get; set; }
        public decimal QRPay { get; set; }
        public decimal LinePay { get; set; }
        public decimal mPayService { get; set; } //new add
        public decimal rabbitTopUp { get; set; } //new add
        public decimal Transportation { get; set; }
        public decimal VASSurcharge { get; set; }
        public decimal Discount { get; set; }
        public decimal Vat { get; set; }
        public decimal Total { get; set; }
        public decimal City { get; set; }
        public decimal Cityn { get; set; }
        public decimal Citys { get; set; }
        public decimal Grab { get; set; }
        public decimal BSDCash { get; set; }
        public decimal BSDLinePay { get; set; }
        public decimal BSDLineTopUp { get; set; }
        public decimal BSDSurcharge { get; set; }
        public decimal BSDCODSurcharge { get; set; }
        public decimal BSDCODAmount { get; set; }
        public decimal BSDCODVasSurcharge { get; set; }
        public decimal BSDCODVat { get; set; }
        public int BSDCODTotalCon { get; set; }
        public int BSDConsignment { get; set; }
        public int BSDBoxes { get; set; }
        public decimal TotalTransfer { get; set; }
        public DateTime? VerifyDate { get; set; }
        public int TotalCon { get; set; }
        public int TotalBoxes { get; set; }
        public int TotalDropOffBoxes { get; set; }
        public string CapturedBy { get; set; }
        public bool VerifyFreight { get; set; }
        public bool VerifyCOD { get; set; }
        public bool VerifyInsurance { get; set; }
        public bool VerifyPackage { get; set; }
        public bool VerifySalePackage { get; set; }
        public bool VerifyLineTopUp { get; set; }
        public bool VerifyDiscount { get; set; }
        public bool VerifyShipments { get; set; }
        public bool VerifyBoxes { get; set; }
        public bool VerifyDropoffBoxes { get; set; }
        public bool VerifyCash { get; set; }
        public bool VerifyRabbit { get; set; }
        public bool VerifyCreditBBL { get; set; }
        public bool VerifyCreditSCB { get; set; }
        public bool VerifyQRPayment { get; set; }
        public bool VerifyLinePay { get; set; }
        public bool VerifyTransportation { get; set; }
        public bool VerifyVASSurcharge { get; set; }
        public bool VerifyVat { get; set; }
    }

    public class DailyRevenueConfirm
    {
        public string ID { get; set; }
        public string ERP_ID { get; set; }
        public string ShopType { get; set; }
        public string BranchID { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal Freight { get; set; }
        public decimal Transport { get; set; }
        public decimal AM { get; set; }
        public decimal PUP { get; set; }
        public decimal SAT { get; set; }
        public decimal RAS { get; set; }
        public decimal COD { get; set; }
        public decimal Insur { get; set; }
        public decimal Pkg { get; set; }
        public decimal SalePackage { get; set; }
        public decimal mPayService { get; set; } //new add
        public decimal rabbitTopUp { get; set; } //new add
        public decimal LineTopUp { get; set; }
        public decimal Cash { get; set; }
        public decimal Rabbit { get; set; }
        public decimal CreditBBL { get; set; }
        public decimal CreditSCB { get; set; }
        public decimal QRPay { get; set; }
        public decimal LinePay { get; set; }
        public decimal Transportation { get; set; }
        public decimal VASSurcharge { get; set; }
        public decimal Discount { get; set; }
        public decimal Vat { get; set; }
        public decimal Total { get; set; }
        public decimal City { get; set; }
        public decimal Cityn { get; set; }
        public decimal Citys { get; set; }
        public decimal Grab { get; set; }
        public decimal BSDLinePay { get; set; }
        public decimal BSDCash { get; set; }
        public decimal BSDLineTopUp { get; set; }
        public decimal BSDSurcharge { get; set; }
        public decimal BSDCODSurcharge { get; set; }
        public decimal BSDCODAmount { get; set; }
        public decimal BSDCODVasSurcharge { get; set; }
        public decimal BSDCODVat { get; set; }
        public int BSDCODTotalCon { get; set; }
        public int BSDConsignment { get; set; }
        public int BSDBoxes { get; set; }
        public decimal TUD { get; set; }
        public string TUDVerifyBy { get; set; }
        public DateTime? TUDVerifyDate { get; set; }
        public decimal TotalTransfer { get; set; }
        public decimal BonusCommission { get; set; }
        public decimal AdjCreditCard { get; set; }
        public decimal AdjOther { get; set; }
        public decimal ReturnCharge { get; set; }
        public decimal Suspense { get; set; }
        public decimal WithHoldingTax { get; set; }
        public decimal Promotion { get; set; }
        public decimal BankCharge { get; set; }
        public decimal AdjLinePay { get; set; }

        //add by kathawutpa 17/7/2019 for project sales x
        public decimal Project { get; set; }

        public int TotalCon { get; set; }
        public int TotalBoxes { get; set; }
        public int TotalDropOffBoxes { get; set; }
        public DateTime? RemittanceDate { get; set; }
        public DateTime? VerifyDate { get; set; }
        public bool Captured { get; set; }
        public DateTime? CapturedDate { get; set; }
        public string CapturedBy { get; set; }
        public bool Approved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovedBy { get; set; }
    }

    public class DailyRevenueConfirmFC
    {
        [Key]
        public string ID { get; set; }
        public string ERP_ID { get; set; }
        public string BranchID { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal Freight { get; set; }
        public decimal Transport { get; set; }
        public decimal AM { get; set; }
        public decimal PUP { get; set; }
        public decimal SAT { get; set; }
        public decimal RAS { get; set; }
        public decimal COD { get; set; }
        public decimal Insur { get; set; }
        public decimal Pkg { get; set; }
        public decimal SalePackage { get; set; }
        public decimal LineTopUp { get; set; }
        public decimal Cash { get; set; }
        public decimal Rabbit { get; set; }
        public decimal CreditBBL { get; set; }
        public decimal CreditSCB { get; set; }
        public decimal QRPay { get; set; }
        public decimal LinePay { get; set; }
        public decimal Transportation { get; set; }
        public decimal VASSurcharge { get; set; }
        public decimal Discount { get; set; }
        public decimal Vat { get; set; }
        public decimal Total { get; set; }
        public decimal City { get; set; }
        public decimal Cityn { get; set; }
        public decimal Citys { get; set; }
        public decimal Grab { get; set; }
        public decimal BSDCash { get; set; }
        public decimal BSDLinePay { get; set; }
        public decimal BSDLineTopUp { get; set; }
        public decimal BSDSurcharge { get; set; }
        public decimal BSDCODSurcharge { get; set; }
        public decimal BSDCODAmount { get; set; }
        public decimal BSDCODVasSurcharge { get; set; }
        public decimal BSDCODVat { get; set; }
        public int BSDCODTotalCon { get; set; }
        public decimal TotalTransfer { get; set; }
        public DateTime? VerifyDate { get; set; }
        public int TotalCon { get; set; }
        public int TotalBoxes { get; set; }
        public bool Approved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool FCConfirmed { get; set; }
        public DateTime? FCConfirmedDate { get; set; }
        public string FCConfirmedBy { get; set; }
    }

    public class DailyRevenueDetail
    {
        [Key]
        public string ID { get; set; }
        public string ERP_ID { get; set; }
        public string BranchID { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal Freight { get; set; }
        public decimal COD { get; set; }
        public decimal Insur { get; set; }
        public decimal Package { get; set; }
        public decimal SalePackage { get; set; }
        public decimal LineTopUp { get; set; }
        public decimal Cash { get; set; }
        public decimal Rabbit { get; set; }
        public decimal Credit { get; set; }
        public decimal CreditSCB { get; set; }
        public decimal CreditBBL { get; set; }
        public decimal QRPay { get; set; }
        public decimal LinePay { get; set; }
        public decimal Transportation { get; set; }
        public decimal VASSurcharge { get; set; }
        public decimal Discount { get; set; }
        public decimal Vat { get; set; }
        public decimal BSDSurcharge { get; set; }
        public decimal City { get; set; }
        public decimal Cityn { get; set; }
        public decimal Citys { get; set; }
        public decimal Grab { get; set; }
        public decimal BSDCash { get; set; }
        public decimal BSDLinePay { get; set; }
        public decimal BSDLineTopUp { get; set; }
        public decimal BSDCODSurcharge { get; set; }
        public decimal BSDCODAmount { get; set; }
        public decimal BSDCODVasSurcharge { get; set; }
        public decimal BSDCODVat { get; set; }
        public int BSDCODTotalCon { get; set; }
        public int BSDConsignment { get; set; }
        public int BSDBoxes { get; set; }
        public decimal DiscountBSD { get; set; }
        public int TotalDropOffBoxes { get; set; }
        public decimal TotalTransfer { get; set; }

        public decimal BonusCommission { get; set; }
        public decimal AdjCreditCard { get; set; }
        public decimal AdjOther { get; set; }
        public decimal ReturnCharge { get; set; }
        public decimal Suspense { get; set; }
        public decimal WithHoldingTax { get; set; }
        public decimal Promotion { get; set; }
        public decimal BankCharge { get; set; }
        public decimal AdjLinePay { get; set; }

        //add by kathawutpa 17/7/2019 for project sales x
        public decimal Project { get; set; }

        // Remark
        public string BonusCommissionRemark { get; set; }
        public string AdjCreditCardRemark { get; set; }
        public string AdjOtherRemark { get; set; }
        public string ReturnChargeRemark { get; set; }
        public string SuspenseRemark { get; set; }
        public string WithHoldingTaxRemark { get; set; }
        public string PromotionRemark { get; set; }
        public string BankChargeRemark { get; set; }
        public string AdjLinePayRemark { get; set; }

        //add by kathawutpa 17/7/2019 for project sales x
        public string ProjectRemark { get; set; }

        public int TotalCon { get; set; }
        public int TotalBoxes { get; set; }

        public DateTime? VerifyDate { get; set; }
        public DateTime? RemittanceDate { get; set; }

        public bool VerifyFreight { get; set; }
        public bool VerifyCOD { get; set; }
        public bool VerifyInsurance { get; set; }
        public bool VerifyPackage { get; set; }
        public bool VerifySalePackage { get; set; }
        public bool VerifyLineTopUp { get; set; }
        public bool VerifyDiscount { get; set; }
        public bool VerifyShipments { get; set; }
        public bool VerifyBoxes { get; set; }
        public bool VerifyDropoffBoxes { get; set; }
        public bool VerifyCash { get; set; }
        public bool VerifyRabbit { get; set; }
        public bool VerifyCreditBBL { get; set; }
        public bool VerifyCreditSCB { get; set; }
        public bool VerifyQRPayment { get; set; }
        public bool VerifyLinePay { get; set; }
        public bool VerifyTransportation { get; set; }
        public bool VerifyVASSurcharge { get; set; }
        public bool VerifyVat { get; set; }
    }

    public class DailyRevenueDetailCash
    {
        [Key]
        public string ID { get; set; }
        public string ERP_ID { get; set; }
        public string BranchID { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal Freight { get; set; }
        public decimal COD { get; set; }
        public decimal Insur { get; set; }
        public decimal Package { get; set; }
        public decimal SalePackage { get; set; }
        public decimal LineTopUp { get; set; }
        public decimal mPayService { get; set; } //new add
        public decimal rabbitTopUp { get; set; } //new add
        public decimal Cash { get; set; }
        public decimal Rabbit { get; set; }
        public decimal Credit { get; set; }
        public decimal CreditSCB { get; set; }
        public decimal CreditBBL { get; set; }
        public decimal QRPay { get; set; }
        public decimal LinePay { get; set; }
        public decimal Transportation { get; set; }
        public decimal VASSurcharge { get; set; }
        public decimal Discount { get; set; }
        public decimal Vat { get; set; }
        public decimal BSDSurcharge { get; set; }
        public decimal City { get; set; }
        public decimal Cityn { get; set; }
        public decimal Citys { get; set; }
        public decimal Grab { get; set; }
        public decimal BSDCash { get; set; }
        public decimal BSDLinePay { get; set; }
        public decimal BSDLineTopUp { get; set; }
        public decimal BSDCODSurcharge { get; set; }
        public decimal BSDCODAmount { get; set; }
        public decimal BSDCODVasSurcharge { get; set; }
        public decimal BSDCODVat { get; set; }
        public int BSDCODTotalCon { get; set; }
        public int BSDConsignment { get; set; }
        public int BSDBoxes { get; set; }
        public decimal DiscountBSD { get; set; }
        public int TotalDropOffBoxes { get; set; }
        public decimal TotalTransfer { get; set; }

        public decimal BonusCommission { get; set; }
        public decimal AdjCreditCard { get; set; }
        public decimal AdjOther { get; set; }
        public decimal ReturnCharge { get; set; }
        public decimal Suspense { get; set; }
        public decimal WithHoldingTax { get; set; }
        public decimal Promotion { get; set; }
        public decimal BankCharge { get; set; }
        public decimal AdjLinePay { get; set; }

        //add by kathawutpa 17/7/2019 for project sales x
        public decimal Project { get; set; }

        // Remark
        public string BonusCommissionRemark { get; set; }
        public string AdjCreditCardRemark { get; set; }
        public string AdjOtherRemark { get; set; }
        public string ReturnChargeRemark { get; set; }
        public string SuspenseRemark { get; set; }
        public string WithHoldingTaxRemark { get; set; }
        public string PromotionRemark { get; set; }
        public string BankChargeRemark { get; set; }
        public string AdjLinePayRemark { get; set; }

        //add by kathawutpa 17/7/2019 for project sales x
        public string ProjectRemark { get; set; }

        public int TotalCon { get; set; }
        public int TotalBoxes { get; set; }

        public DateTime? VerifyDate { get; set; }
        public DateTime? RemittanceDate { get; set; }

        public bool VerifyFreight { get; set; }
        public bool VerifyCOD { get; set; }
        public bool VerifyInsurance { get; set; }
        public bool VerifyPackage { get; set; }
        public bool VerifySalePackage { get; set; }
        public bool VerifyLineTopUp { get; set; }
        public bool VerifyDiscount { get; set; }
        public bool VerifyShipments { get; set; }
        public bool VerifyBoxes { get; set; }
        public bool VerifyDropoffBoxes { get; set; }
        public bool VerifyCash { get; set; }
        public bool VerifyRabbit { get; set; }
        public bool VerifyCreditBBL { get; set; }
        public bool VerifyCreditSCB { get; set; }
        public bool VerifyQRPayment { get; set; }
        public bool VerifyLinePay { get; set; }
        public bool VerifyTransportation { get; set; }
        public bool VerifyVASSurcharge { get; set; }
        public bool VerifyVat { get; set; }

        public int CheckReconcile { get; set; }
    }
}

