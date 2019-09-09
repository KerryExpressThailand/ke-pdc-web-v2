using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models
{
    public class ShopDailyRevenue
    {
        [Key]
        public string BranchID { get; set; }
        public string erp_id { get; set; }
        public int MtdSender { get; set; }
        public int MtdCon { get; set; }
        public int MtdBox { get; set; }
        public decimal MtdRevenue { get; set; }
        public decimal MtdYPC { get; set; }
        public decimal MtdYPB { get; set; }
        public int DailySender { get; set; }
        public int DailyCon { get; set; }
        public int DailyBox { get; set; }
        public decimal DailyRevenue { get; set; }
        public decimal DailyYPC { get; set; }
        public decimal DailyYPB { get; set; }
    }
}
