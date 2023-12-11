using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public partial class FoodItem : ObservableObject
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; }
        public string Type { get; set; }
        [ObservableProperty, NotifyPropertyChangedFor(nameof(Amount))]
        private int _cartQuantity;
        public double Amount => CartQuantity * Price;
        public FoodItem Clone() => MemberwiseClone() as FoodItem;
    }
}
