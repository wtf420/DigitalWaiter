using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    [QueryProperty(nameof(FoodItem), nameof(FoodItem))]
    public partial class DetailsViewModel : ObservableObject, IDisposable
    {
        private readonly CartViewModel _cartViewModel;
        public DetailsViewModel(CartViewModel cartViewModel) {
            _cartViewModel = cartViewModel;
            _cartViewModel.CartCleared += OnCartCleared;
            _cartViewModel.CartItemRemoved += OnCartItemRemoved;
            _cartViewModel.CartItemUpdated += OnCartItemUpdated;
        }

        private void OnCartCleared(object? _, EventArgs e) => FoodItem.CartQuantity = 0;
        private void OnCartItemRemoved(object? _, FoodItem p) => OnCartItemChanged(p, 0);
        private void OnCartItemUpdated(object? _, FoodItem p) => OnCartItemChanged(p, p.CartQuantity);
        private void OnCartItemChanged(FoodItem p, int quantity)
        {
            if (p.Name == FoodItem.Name)
            {
                FoodItem.CartQuantity = quantity;
            }
        }

        [ObservableProperty]
        private FoodItem _foodItem;

        [RelayCommand]
        private void AddToCart()
        {
            FoodItem.CartQuantity++;
            _cartViewModel.UpdateCartItemCommand.Execute(FoodItem);
        }

        [RelayCommand]
        private void RemoveFromCart()
        {
            if (FoodItem.CartQuantity > 0)
            {
                FoodItem.CartQuantity--;
                _cartViewModel.UpdateCartItemCommand.Execute(FoodItem);
            }
        }

        [RelayCommand]
        private async Task ViewCart()
        {
            if (FoodItem.CartQuantity > 0)
            {
                await Shell.Current.GoToAsync(nameof(CartPage), animate: true);
            } else
            {
                await Toast.Make("Please select quantity!", ToastDuration.Short).Show();
            }
        }

        public void Dispose()
        {
            _cartViewModel.CartCleared -= OnCartCleared;
            _cartViewModel.CartItemRemoved -= OnCartItemRemoved;
            _cartViewModel.CartItemUpdated -= OnCartItemUpdated;
        }
    }
}
