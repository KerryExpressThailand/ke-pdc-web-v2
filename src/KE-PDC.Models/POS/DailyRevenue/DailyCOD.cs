using System;
using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class DailyCOD
    {
        [Key]
        public string BranchID { get; set; }
        public string ERPID { get; set; }
        public int MonthlyCODConsignment { get; set; }
        public decimal MonthlyCODAmount { get; set; }
        public decimal MonthlyCODSurcharge { get; set; }
        public decimal MonthlyAmountPerConsignment { get; set; }
        public int DailyCODConsignment { get; set; }
        public decimal DailyCODAmount { get; set; }
        public decimal DailyCODSurcharge { get; set; }
        public decimal DailyAmountPerConsignment { get; set; }
    }

    public class DailyCODDetail
    {
        [Key]
        public string Consignment { get; set; }
        public string BranchID { get; set; }
        public string AccountID { get; set; }
        public decimal CODAmount { get; set; }
        public DateTime PickupDate { get; set; }
    }
}
