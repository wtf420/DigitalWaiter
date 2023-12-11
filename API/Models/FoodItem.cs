namespace API.Models
{
    public class FoodItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; }
        public string Type { get; set; }
    }
}
