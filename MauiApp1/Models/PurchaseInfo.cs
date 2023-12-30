using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class PurchaseInfo
    {
        public long Id { get; set; }
        public long OrderItemId { get; set; }
        public long FoodItemId { get; set; }
        public long Quantity { get; set; }
        public double Price { get; set; }
    }
}
