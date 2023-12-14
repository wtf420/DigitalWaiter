using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    public partial class CartViewModel : ObservableObject
    {
        public event EventHandler<FoodItem> CartItemRemoved;
        public event EventHandler<FoodItem> CartItemUpdated;
        public event EventHandler CartCleared;
        public ObservableCollection<FoodItem> Items { get; set; } = new();

        [ObservableProperty]
        private double _totalAmount;
        private void RecalculateTotalAmount() => TotalAmount = Items.Sum(i => i.Amount);
        [RelayCommand]
        private void UpdateCartItem(FoodItem foodItem)
        {
            var item = Items.FirstOrDefault(i => i.Name == foodItem.Name);
            if (item != null)
            {
                item.CartQuantity = foodItem.CartQuantity;
            }
            else
            {
                Items.Add(foodItem.Clone());
            }
            RecalculateTotalAmount();
        }

        [RelayCommand]
        private async void RemoveCartItem(string name)
        {
            var item = Items.FirstOrDefault(i => i.Name == name);
            if (item != null)
            {
                Items.Remove(item);
                RecalculateTotalAmount();

                CartItemRemoved?.Invoke(this, item);

                var snackbarOptions = new SnackbarOptions
                {
                    CornerRadius = 10,
                    BackgroundColor = Colors.PaleGoldenrod
                };
                var snackbar = Snackbar.Make($"'{item.Name}' removed from cart.",
                    () =>
                    {
                        Items.Add(item);
                        RecalculateTotalAmount();
                        CartItemUpdated(this, item);
                    }, "Undo", TimeSpan.FromSeconds(5), snackbarOptions);
                await snackbar.Show();
            }
        }

        [RelayCommand]
        private async void ClearCart()
        {
            if (await Shell.Current.DisplayAlert("Confirm clear cart?",
                "Do you really want to clear cart?",
                "Yes", "no"))
            {
                Items.Clear();
                RecalculateTotalAmount();
                CartCleared?.Invoke(this, EventArgs.Empty);
                await Toast.Make("Cart Cleared", ToastDuration.Short).Show();
            }
        }

        [RelayCommand]
        private async Task PlaceOrder()
        {
            var item = new OrderItem
            {
                OrderFoodItems = new(Items),
                ExtraNote = "Spicy",
                Date = DateTime.Now.ToString(),
                Price = TotalAmount,
                Completed = false
            };
            bool result = await ServiceHelper.GetService<OrderService>().PlaceOrder(item);
            if (result)
            {
                Items.Clear();
                CartCleared.Invoke(this, EventArgs.Empty);
                RecalculateTotalAmount();

                await Shell.Current.GoToAsync(nameof(CheckoutPage), animate: true);
            }
        }
    }
}
