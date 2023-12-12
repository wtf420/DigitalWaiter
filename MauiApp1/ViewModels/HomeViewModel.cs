using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Models;
using MauiApp1.Services;
using System;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly FoodService _foodService;
        public HomeViewModel(FoodService foodService)
        {
            _foodService = foodService;
            Task.Run(async () =>
            {
                await ServiceHelper.GetService<FoodService>().RefreshDataAsync();
                FoodItems = new(_foodService.GetAllFoodItems());
                OnPropertyChanged(nameof(FoodItems));
            });
        }

        public ObservableCollection<FoodItem> FoodItems { get; set;}

        [RelayCommand]
        private async Task GoToAllPizzasPage(bool fromSearch = false)
        {
            var parameters = new Dictionary<string, object>
            {
                [nameof(AllPizzasViewModel.FromSearch)] = fromSearch
            };
            await Shell.Current.GoToAsync(nameof(AllPizzasPage), animate: true, parameters);
        }

        [RelayCommand]
        private async Task GoToDetailsPage(FoodItem foodItem)
        {
            var parameters = new Dictionary<string, object>
            {
                [nameof(DetailsViewModel.FoodItem)] = foodItem
            };
            await Shell.Current.GoToAsync(nameof(DetailPage), animate: true, parameters);
        }
    }
}
