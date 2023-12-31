﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Task.Run(RefreshDataAsync);
            /*_foodItems = new List<FoodItem>
            {
                new FoodItem
                {
                    Name = "Peppino's Pizza",
                    Image = "pizzafull.png",
                    Price = 100,
                    IsAvailable = true,
                    Type = "Food"
                },
                new FoodItem
                {
                    Name = "Gustavo's Pizza",
                    Image = "pizzafull.png",
                    Price = 120,
                    IsAvailable = true,
                    Type = "Food"
                },
            };*/
        }

        public async Task RefreshDataAsync()
        {
            var httpclient = new HttpClient();
            var response = await httpclient.GetAsync(ServiceHelper.ConnectionURL + "api/FoodItems");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                List<FoodItem> jsonDataCollection = JsonConvert.DeserializeObject<List<FoodItem>>(data);
                _foodItems = jsonDataCollection;
            } else
            {
                await Toast.Make(response.StatusCode.ToString(), ToastDuration.Short).Show();
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
