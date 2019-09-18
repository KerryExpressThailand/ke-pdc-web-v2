using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KE_PDC.Models
{
    public class LineOfBillPayModel
    {
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
        public string F { get; set; }
        public string G { get; set; }
        public string H { get; set; }
        public string I { get; set; }
        public string J { get; set; }
        public string K { get; set; }
        public string L { get; set; }
    }

    public class HeaderBillPayModel
    {
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
        public string F { get; set; }
        public string G { get; set; }
        public string H { get; set; }
        public string I { get; set; }
        public string J { get; set; }
        public string K { get; set; }
        public string L { get; set; }
    }

    public class BillPayModel
    {

        public string Id { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? PayDate { get; set; }
        public TimeSpan? PayTime { get; set; }
        public string PayBy { get; set; }
        public string ReferenceNo { get; set; }
        public string FrBr { get; set; }
        public decimal? Amount { get; set; }
        public string ChqNo { get; set; }
        public string Bc { get; set; }
        public string Rc { get; set; }
    }

    public class NoBillPayModel
    {

        public string Id { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime? PayDate { get; set; }
        public decimal? Amount { get; set; }
    }

    public class NoBill
    {

        public string Id { get; set; }
        public string BatchId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime? PayDate { get; set; }
        public decimal? Amount { get; set; }
        public string CreatedBy { get; set; }
    }


    public class BillPay
    {

        public string Id { get; set; }
         public string BatchId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? PayDate { get; set; }
        public TimeSpan? PayTime { get; set; }
        public string PayBy { get; set; }
        public string ReferenceNo { get; set; }
        public string FrBr { get; set; }
        public decimal? Amount { get; set; }
        public string ChqNo { get; set; }
        public string Bc { get; set; }
        public string Rc { get; set; }
        public string CreatedBy { get; set; }
       
    }

    public class CheckRecode
    {

        public int TotalRecord { get; set; }
        public string BatchId { get; set; }

    }

    public class RevenueData
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


}
