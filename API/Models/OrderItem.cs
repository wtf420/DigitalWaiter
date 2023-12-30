using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class OrderItem
    {
        public long Id { get; set; }
        public string? ExtraNote { get; set; }
        public bool Completed { get; set; }
        public double TotalPrice { get; set; }
        public string Date { get; set; }
        public virtual List<PurchaseInfo> PurchasedItems { get; set; }
    }
}
