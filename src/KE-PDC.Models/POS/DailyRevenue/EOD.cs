using System;

namespace KE_PDC.Models
{
    public class EOD
    {
        public string BranchID { get; set; }
        public string BranchName { get; set; }
        public string BranchType { get; set; }
        public string ERPID { get; set; }
        public DateTime Report_Date { get; set; }
        public decimal TotalTransfer { get; set; }
        public int Shipment { get; set; }
        public int Boxes { get; set; }
        public decimal TransportService { get; set; }
        public decimal AMService { get; set; }
        public decimal PUPService { get; set; }
        public decimal SATService { get; set; }
        public decimal RASService { get; set; }
        public decimal CODService { get; set; }
        public decimal INSURService { get; set; }
        public decimal PACKAGEService { get; set; }
        public decimal SALEService { get; set; }
        public decimal LNTUPService { get; set; }
        public int DropOffBoxes { get; set; }
        public decimal TotalDetailService { get; set; }
        public decimal DHLService { get; set; }
        public decimal Cash { get; set; }
        public decimal Rabbit { get; set; }
        public decimal Credit { get; set; }
        public decimal CreditBBL { get; set; }
        public decimal CreditSCB { get; set; }
        public decimal QRPay { get; set; }
        public decimal LinePay { get; set; }
        public decimal TotalDetailPay { get; set; }
        public decimal Transportation { get; set; }
        public decimal VASSurcharge { get; set; }
        public decimal VAT { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalDetailSurcharge { get; set; }
        public decimal TotalFreightRevenue { get; set; }
        public string JSON_Data { get; set; }
        public bool? SFTP_Process { get; set; }
        public DateTime? SFTP_Date { get; set; }
        public bool? Processing { get; set; }
        public bool? Processed { get; set; }
        public DateTime? Processed_Date { get; set; }
        public int TotalShipments { get; set; }
        public int TotalBoxes { get; set; }
        public DateTime? LastedUpdate { get; set; }

        public decimal BSDCity { get; set; }
        public decimal BSDCityn { get; set; }
        public decimal BSDCitys { get; set; }
        public decimal BSDGrab { get; set; }
        public decimal BSDCash { get; set; }
        public decimal BSDLinePay { get; set; }
        public decimal BSDLineTopUp { get; set; }
        public int BSDConsignment { get; set; }
        public int BSDBoxes { get; set; }
        public decimal BSDDiscount { get; set; }
        public decimal BSDTotalDetailService { get; set; }
        public decimal BSDTotalPayment { get; set; }
        public decimal BSDTotalPaymentCash { get; set; }
    }
}
