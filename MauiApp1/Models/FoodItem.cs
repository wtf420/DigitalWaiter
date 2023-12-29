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
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? IncludedItems { get; set; }
        public bool IsAvailable { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; }
        public string Type { get; set; }

        public ImageSource Picture {
            get
            {
                var imageBytes = Convert.FromBase64String(Image);

                MemoryStream imageDecodeStream = new(imageBytes);
                
                return ImageSource.FromStream(() => imageDecodeStream);
            }
            set { return; }
        }

        [ObservableProperty, NotifyPropertyChangedFor(nameof(Amount))]
        private int _cartQuantity;
        public double Amount => CartQuantity * Price;
        public FoodItem Clone() => MemberwiseClone() as FoodItem;
    }
}
