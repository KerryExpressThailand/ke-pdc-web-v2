using System;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class NoneShipmentDHL
    {
        public int RecordID { get; set; }
        public string BranchID { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal BeforeVat { get; set; }
        public decimal TotalVat { get; set; }
        public decimal Amount { get; set; }
    }
}
