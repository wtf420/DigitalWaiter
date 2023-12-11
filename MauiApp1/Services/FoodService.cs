using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace MauiApp1.Services
{
    public class FoodService
    {
        public FoodService()
        {
            RefreshDataAsync();
        }

        public async void RefreshDataAsync()
        {
            var httpclient = new HttpClient();
            var response = await httpclient.GetAsync("http://localhost:5229/api/FoodItems");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var jsonDataCollection = JsonConvert.DeserializeObject<List<FoodItem>>(data);
                _foodItems = jsonDataCollection;
            }
        }

        private static IEnumerable<FoodItem> _foodItems = new List<FoodItem>();

        public IEnumerable<FoodItem> GetAllFoodItems() => _foodItems;

        public IEnumerable<FoodItem> GetPopularFoodItems(int count = 6) =>
            _foodItems.OrderBy(p => Guid.NewGuid()).Take(count);

        public IEnumerable<FoodItem> SearchFoodItems(string searchTerm) =>
            string.IsNullOrWhiteSpace(searchTerm) ?
            _foodItems : _foodItems.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }
}
