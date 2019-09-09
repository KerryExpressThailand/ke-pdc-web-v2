using System;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class LINETopUp
    {
        [Key]
        public Int64 No { get; set; }
        public string transaction_id { get; set; }
        public DateTime transaction_date { get; set; }
        public decimal amount { get; set; }
        public string Status { get; set; }
        public string order_id { get; set; }
        public string location_id { get; set; }
        public string return_code { get; set; }
    }
}
