using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MauiApp1.Services
{
    public class OrderService
    {
        private bool a = true;
        public OrderService() 
        {
            
        }

        public void StartUpdating()
        {
            Task.Run(async () =>
            {
                Debug.WriteLine("Refreshing");
                while (a)
                {
                    if (_orderItems.Count != 0)
                    {
                        await RefreshDataAsync();
                        await Task.Delay(1000);
                    }
                }
            });
        }

        private static List<OrderItem> _orderItems = new List<OrderItem>();
        public List<OrderItem> GetAllFoodItems() => _orderItems;

        public async Task RefreshDataAsync()
        {
            var httpclient = new HttpClient();
            var response = await httpclient.GetAsync(ServiceHelper.ConnectionURL + "api/OrderItems");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var jsonDataCollection = JsonConvert.DeserializeObject<List<OrderItem>>(data);
                foreach ( var item in _orderItems) 
                {
                    var order = jsonDataCollection.FirstOrDefault(x => x.Id == item.Id);
                    if (order.Completed != item.Completed)
                    {
                        _orderItems.Remove(item);
                        await Toast.Make("One of your orders are completed!", ToastDuration.Short).Show();
                    }
                }
            }
        }

        public async Task<bool> PlaceOrder(OrderItem orderItem)
        {
            var httpclient = new HttpClient();
            var _serializerOptions = new JsonSerializerOptions();
            string json = System.Text.Json.JsonSerializer.Serialize<OrderItem>(orderItem, _serializerOptions);
            Debug.WriteLine(json);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpclient.PostAsync(ServiceHelper.ConnectionURL + "api/OrderItems", content);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<OrderItem>(data);
                _orderItems.Add(order);
                return true;
            } else
            {
                await Toast.Make(response.StatusCode.ToString(), ToastDuration.Short).Show();
                return false;
            }
        }
    }
}
