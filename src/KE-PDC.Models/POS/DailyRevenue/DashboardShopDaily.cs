using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KE_PDC.Models
{
    public class DashboardShopDaily
    {
        [Key]
        public string ReportDate { get; set; }
        public int? TotalConsignment { get; set; }
        public int? TotalBox { get; set; }
        public decimal? TotalDHL { get; set; }
        public decimal? TotalRevenue { get; set; }
        public decimal? TotalTransferPOS { get; set; }
        public decimal? TotalTransferBSD { get; set; }
        public decimal? TotalTransfer { get; set; }
        public decimal? TotalCash { get; set; }
        public decimal? TotalRabbit { get; set; }
        public decimal? TotalCredit { get; set; }
        public decimal? TotalLinePay { get; set; }
        public decimal TotalRevenueOnlyFreight { get; set; }
        public decimal? TotalCOD { get; set; }
        public decimal? TotalInsurance { get; set; }
        public decimal? TotalBSD { get; set; }
        public decimal? TotalLineTopUp { get; set; }
        public decimal? TotalPackage { get; set; }
        public decimal TotalValueAddedService { get; set; }
    }

    public class CashReport
    {
        [Key]
        public string ConsignmentNo { get; set; }
        public string BranchID { get; set; }
        public string BranchName { get; set; }
        public string PayerAddress1 { get; set; }
        public string PayerAddress2 { get; set; }
        public string PayerZipcode { get; set; }
        public string PayerTelephone { get; set; }
        public string SenderName { get; set; }
        public string SenderTelephone { get; set; }
        public string RecipientName { get; set; }
        public string RecipientAddress1 { get; set; }
        public string RecipientAddress2 { get; set; }
        public string RecipientZipcode { get; set; }
        public string RecipientTelephone { get; set; }
        public string RecipientContactPerson { get; set; }
        public string ServiceCode { get; set; }
        public string ParcelSize { get; set; }
        public double Weight { get; set; }
        public decimal Surcharge { get; set; }
        public decimal VASSurcharge { get; set; }
        public decimal Discount { get; set; }
        public decimal Vat { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string TaxInvoiceNo { get; set; }
        public int Quantity { get; set; }
        public string CollectType { get; set; }
        public decimal DeclareValue { get; set; }
        public decimal CODAmount { get; set; }
        public string CODAccountID { get; set; }
        public decimal SurchargeCOD { get; set; }
        public decimal SurchargeINSUR { get; set; }
        public decimal SurchargePKG { get; set; }
        public decimal SurchargeAM { get; set; }
        public decimal SurchargePUP { get; set; }
        public decimal SurchargeTRANS { get; set; }
        public decimal SurchargeRAS { get; set; }
        public decimal SurchargeSAT { get; set; }
        public string CI { get; set; }
        public string SEALNO { get; set; }
    }

    public class Invoice
    {
        public string TaxInvoiceDate { get; set; }
        [Key]
        public string TaxInvoiceNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerTaxID { get; set; }
        public string HeadOffice { get; set; }
        public string BranchName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalVat { get; set; }
        public decimal GrandTotal { get; set; }
        public string Type { get; set; }
    }

    public class LinePayInvoice
    {
        public string TaxInvoiceDate { get; set; }
        public string TaxInvoiceTime { get; set; }
        [Key]
        public string TaxInvoiceNo { get; set; }
        public string LineTransactionNo { get; set; }
        public string ReceiptNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerTaxID { get; set; }
        public string CustomerHeadOffice { get; set; }
        public string CustomerBranchName { get; set; }
        public decimal Amount { get; set; }
        public decimal Vat { get; set; }
        public decimal GrandTotal { get; set; }
        public string ConsignmentList { get; set; }
    }

    public class LinePayTopup
    {
        [Key]
        public int RunNo { get; set; }
        public string VASID { get; set; }
        public string OrderID { get; set; }
        public string CustomerQRCode { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string By { get; set; }
        public string ClientName { get; set; }
        public string TransactionID { get; set; }
        public string TransactionDate { get; set; }
        public int BatchNo { get; set; }

    }

    public class BoxRevenue
    {
        [Key]
        public string TaxInvoiceNo { get; set; }
        public string TaxInvoiceDate { get; set; }
        public string ReceivedType { get; set; }
        public string ProductID { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public string ConsignmentNo { get; set; }
        public string Cancel { get; set; }
        public string CanceledRemark { get; set; }
        public string CanceledBy { get; set; }
        public string CanceledDate { get; set; }
        public string Type { get; set; }
    }

    public class MDEConsignments
    {
        [Key]
        public string ConsignmentNo { get; set; }
        public string BookingNo { get; set; }
        public string RefNo { get; set; }
        public string PayerID { get; set; }
        public string PayerName { get; set; }
        public string PayerAddress1 { get; set; }
        public string PayerAddress2 { get; set; }
        public string PayerZipcode { get; set; }
        public string PayerTelephone { get; set; }
        public string PayerFax { get; set; }
        public string PaymentMode { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress1 { get; set; }
        public string SenderAddress2 { get; set; }
        public string SenderZipcode { get; set; }
        public string SenderTelephone { get; set; }
        public string SenderFax { get; set; }
        public string SenderContactPerson { get; set; }
        public string RecipientName { get; set; }
        public string RecipientAddress1 { get; set; }
        public string RecipientAddress2 { get; set; }
        public string RecipientZipcode { get; set; }
        public string RecipientTelephone { get; set; }
        public string RecipientFax { get; set; }
        public string RecipientContactPerson { get; set; }
        public string ServiceCode { get; set; }
        public decimal DeclareValue { get; set; }
        public string PaymentType { get; set; }
        public string CommodityCode { get; set; }
        public string Remark { get; set; }
        public decimal CODAmount { get; set; }
        public string ReturnPODHC { get; set; }
        public string ReturnInvoiceHC { get; set; }
    }

    public class MDEPackages
    {
        [Key]
        public string ConsignmentNo { get; set; }
        public int MPSCode { get; set; }
        public int PackageLength { get; set; }
        public int PackageBreadth { get; set; }
        public int PackageHeight { get; set; }
        public double PackageWeight { get; set; }
        public int PackageQuantity { get; set; }
    }
}
