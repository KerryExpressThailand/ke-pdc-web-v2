using System;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class DailyRevenueDHL
    {
        public string BranchID { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal DHLService { get; set; }
    }
}
