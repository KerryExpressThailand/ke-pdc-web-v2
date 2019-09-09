using System;

namespace KE_PDC.Models
{
    public class DailyRevenuePivot
    {
        public string ShopType { get; set; }
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
        public decimal BBL { get; set; }
        public decimal SCB { get; set; }
        public decimal LinePay { get; set; }
        public decimal Discount { get; set; }
        public decimal Vat { get; set; }
        public decimal Total { get; set; }
        public decimal BSDSurcharge { get; set; }
        //public decimal City { get; set; }
        //public decimal Cityn { get; set; }
        //public decimal Citys { get; set; }
        //public decimal Bsd { get; set; }
        //public decimal Grab { get; set; }
        public decimal BSDCash { get; set; }
        public decimal BSDLinePay { get; set; }
        public decimal BSDTopup { get; set; }
        public decimal BSDCod { get; set; }
        public decimal BSDVas { get; set; }
        public decimal BSDVat { get; set; }
        public int BSDCon { get; set; }
        public int BSDBoxes { get; set; }
        public decimal TUD { get; set; }
        public decimal TotalTransfer { get; set; }
        public int TotalCon { get; set; }
        public int TotalBoxes { get; set; }
        public DateTime RemittanceDate { get; set; }
        public DateTime VerifyDate { get; set; }
    }
}
