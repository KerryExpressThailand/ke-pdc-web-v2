using System;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class Receipt
    {
        [Key]
        public string ReceiptNo { get; set; }
        public string ERP_ID { get; set; }
        public string BranchID { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public string ReceiptBranchNo { get; set; }
        public string BranchName { get; set; }
        public string BranchType { get; set; }
        public string CustomerName { get; set; }
        public string CustomerTaxID { get; set; }
        public string CustomerIsHQ { get; set; }
        public string CustomerBranchName { get; set; }
        public decimal Amount { get; set; }
        public string Canceled { get; set; }
    }
}
