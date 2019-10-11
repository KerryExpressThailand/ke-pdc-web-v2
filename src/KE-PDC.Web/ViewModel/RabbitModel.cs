using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KE_PDC.ViewModel
{
    public class RabbitTransaction
    {
        public string transaction_id { get; set; }
        public string payment_method { get; set; }
        public DateTime? transaction_date { get; set; }
        public string item_amount { get; set; }
        public string discount_amount { get; set; }
        public string payment_amount { get; set; }
        public string transaction_type { get; set; }
        public string payment_status { get; set; }
        public string capture_schema { get; set; }
        public string merchant_id { get; set; }
        public string branch_id { get; set; }
        public string device_id { get; set; }
        public string transaction_ref { get; set; }
    }
}
