using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class OrderItem
    {
        public long Id { get; set; }
        public string? ExtraNote { get; set; }
        public double Price { get; set; }
        public string Date { get; set; }
        public virtual List<FoodItem> OrderFoodItems { get; set; }
    }
}
