using MauiApp1.Models;

namespace MauiApp1.Services
{
    public class PizzaService
    {
        private readonly static IEnumerable<Pizza> _pizzzas = new List<Pizza>
        {
            new Pizza
            {
                Name = "Peppino Pizza",
                Image = "pizzafull.png",
                Price = 5.1
            },
            new Pizza
            {
                Name = "Gustavo Pizza",
                Image = "pizzafull.png",
                Price = 3.8
            },
        };

        public IEnumerable<Pizza> GetAllPizzas() => _pizzzas;

        public IEnumerable<Pizza> GetPopularPizzas(int count = 6) =>
            _pizzzas.OrderBy(p => Guid.NewGuid()).Take(count);

        public IEnumerable<Pizza> SearchPizzas(string searchTerm) =>
            string.IsNullOrWhiteSpace(searchTerm) ?
            _pizzzas : _pizzzas.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }
}
