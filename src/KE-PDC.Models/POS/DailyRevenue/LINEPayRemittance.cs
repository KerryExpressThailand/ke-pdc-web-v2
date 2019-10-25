using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class LINEPayRemittance
    {
        [Key]
        public string id { get; set; }
        public string BranchID { get; set; }
        public DateTime ReportDate { get; set; }
        public string DMSID { get; set; }
        public string branch_type { get; set; }
        public string ERP_ID { get; set; }
        public string BranchName { get; set; }
        public decimal TUC { get; set; }
        public decimal TUP { get; set; }
        public decimal TUD { get; set; }
        public string Captured { get; set; }
        public string CapturedBy { get; set; }
        public DateTime? CapturedDate { get; set; }
        public string TUDVerifyBy { get; set; }
        public DateTime? TUDVerifyDate { get; set; }
        public DateTime? RemittanceDate { get; set; }
    }

    public class Remittance
    {
        public string RemittanceDate { get; set; }
    }


    public class ResultRemittancedetail
    {
        public List<Remittancedetail> Result { get; set; }
    }
    public class Remittancedetail
    {
        [Key]
        public string id { get; set; }
        public string BranchID { get; set; }
        public DateTime ReportDate { get; set; }
        public string DMSID { get; set; }
        public string branch_type { get; set; }
        public string ERP_ID { get; set; }
        public string BranchName { get; set; }
        public decimal TUC { get; set; }
        public decimal TUP { get; set; }
        public decimal TUD { get; set; }
        public string Captured { get; set; }
        public string CapturedBy { get; set; }
        public DateTime? CapturedDate { get; set; }
        public DateTime RemittanceDate { get; set; }
        public decimal unit_price { get; set; }
        public string MPay_transaction_Id { get; set; }
        public DateTime? created_datetime { get; set; }
       
    }

    public class ResultRemittancedetailRabbit
    {
        public List<Remittancedetail> Result { get; set; }
    }
    public class RabbitTopUpRabbit
    {
        [Key]
        public string id { get; set; }
        public string BranchID { get; set; }
        public DateTime ReportDate { get; set; }
        public string DMSID { get; set; }
        public string branch_type { get; set; }
        public string ERP_ID { get; set; }
        public string BranchName { get; set; }
        public decimal TUC { get; set; }
        public decimal TUP { get; set; }
        public decimal TUD { get; set; }
        public string Captured { get; set; }
        public string CapturedBy { get; set; }
        public DateTime? CapturedDate { get; set; }
        public DateTime RemittanceDate { get; set; }
        public decimal unit_price { get; set; }
        public string Rabbit_transaction_Id { get; set; }
        public DateTime? created_datetime { get; set; }

    }
}
