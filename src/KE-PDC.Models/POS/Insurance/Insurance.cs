using System;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models.POS
{
    public class Insurance
    {
        [Key]
        public string ConsignmentNo { get; set; }
        public string SenderName { get; set; }
        public string SenderMobile { get; set; }
        public string IDCard { get; set; }
        public string RecipientName { get; set; }
        public string RecipientMobile { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientSoi { get; set; }
        public string RecipientRoad { get; set; }
        public string RecipientDistrict { get; set; }
        public string RecipientAmphur { get; set; }
        public string RecipientProvince { get; set; }
        public string RecipientZipcode { get; set; }
        public double ParcelWeight { get; set; }
        public decimal TotalAmount { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string ReceiptNo { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int Qty { get; set; }
        public decimal InsuranceAmount { get; set; }
        public string InsuranceRemark { get; set; }
        public decimal CODAmount { get; set; }
        public string AccountId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
