namespace API.Models
{
    public class OrderItem
    {
        public long Id { get; set; }
        public List<FoodItem> OrderFoodItems { get; set; }
        public string? ExtraNote { get; set; }
        public double Price { get; set; }
        public string Date { get; set; }
    }
}
