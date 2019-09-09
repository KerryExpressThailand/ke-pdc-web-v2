using System.ComponentModel.DataAnnotations;

namespace KE_PDC.Models.POS.Stock
{
    public class StockStatus
    {
        [Key]
        public int ID { get; set; }
        public string Description { get; set; }
    }
}
