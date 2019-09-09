using System;
using System.Collections.Generic;
using System.Text;

namespace KE_PDC.Models.POS
{
    public class ReconcileAdjustTransaction
    {
        public string BranchId { get; set; }
        public DateTime ReportDate { get; set; }
        public string TypeId { get; set; }
        public string AdjustId { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
