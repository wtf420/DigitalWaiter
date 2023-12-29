using System.Drawing;

namespace API.Models
{
    public class FoodItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? IncludedItems { get; set; }
        public bool IsAvailable { get; set; } = false;
        public string? Image { get; set; }
        public double Price { get; set; }
        public string? Type { get; set; }
    }
}
