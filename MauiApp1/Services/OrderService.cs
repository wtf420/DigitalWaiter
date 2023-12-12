using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MauiApp1.Services
{
    public class OrderService
    {
        public OrderService() { }
        private static IEnumerable<OrderItem> _orderItems = new List<OrderItem>();
        public IEnumerable<OrderItem> GetAllFoodItems() => _orderItems;
        public async Task<bool> PlaceOrder(OrderItem orderItem)
        {
            var httpclient = new HttpClient();
            var _serializerOptions = new JsonSerializerOptions();
            string json = JsonSerializer.Serialize<OrderItem>(orderItem, _serializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpclient.PostAsync("http://localhost:5229/api/OrderItems", content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            } else
            {
                await Toast.Make(response.Content.ToString(), ToastDuration.Short).Show();
                return false;
            }
        }
    }
}
