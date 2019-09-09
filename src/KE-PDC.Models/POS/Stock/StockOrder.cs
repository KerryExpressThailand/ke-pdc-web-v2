using System;

namespace KE_PDC.Models
{
    public class StockOrder
    {
        public string BranchType { get; set; }
        public string ERP_ID { get; set; }
        public string BranchID { get; set; }
        public int OrderID { get; set; }
        public int Status { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string PackageID { get; set; }
        public string PackageDescription { get; set; }
        public string Unit { get; set; }
        public int OrderQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int PackingQuantity { get; set; }
        public decimal Amount { get; set; }
    }
}
