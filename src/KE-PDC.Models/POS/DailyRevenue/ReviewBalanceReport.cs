using System;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class ReviewBalanceReport
    {
        [Key]
        public Guid ID { get; set; }
        public string BranchID { get; set; }
        public DateTime? ReportDate { get; set; }
        public string ShopType { get; set; }
        public string OracleDC { get; set; }
        public DateTime? SaleDate { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public string OracleAccount { get; set; }
        public string ItemName { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Total { get; set; }
        public string Remark { get; set; }
        public string SortBy { get; set; }
        public string Company { get; set; }
        public string Account { get; set; }
        public string BU { get; set; }
        public string CostCenter { get; set; }
        public string DcDel { get; set; }
        public string Truck { get; set; }
        public string Reserve1 { get; set; }
        public string Reserve2 { get; set; }
        public string Reserve3 { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
