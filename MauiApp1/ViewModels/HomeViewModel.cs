﻿using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Models;
using MauiApp1.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MauiApp1.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly FoodService _foodService;
        public bool refresh = true;
        public string a { get; set; }

        public HomeViewModel(FoodService foodService)
        {
            refresh = true;
            _foodService = foodService;
            a = "0";
        }

        public async void Update()
        {
            await ServiceHelper.GetService<FoodService>().RefreshDataAsync();
            FoodItems = new(_foodService.GetAllFoodItems());
            OnPropertyChanged(nameof(FoodItems));
            a = ServiceHelper.GetService<CartViewModel>().Items.Count.ToString();
            OnPropertyChanged(nameof(a));
        }

        public ObservableCollection<FoodItem> FoodItems { get; set;}

        [RelayCommand]
        private async Task GoToAllPizzasPage(bool fromSearch = false)
        {
            refresh = false;
            var parameters = new Dictionary<string, object>
            {
                [nameof(AllPizzasViewModel.FromSearch)] = fromSearch
            };
            await Shell.Current.GoToAsync(nameof(AllPizzasPage), animate: true, parameters);
        }

        [RelayCommand]
        private async Task GoToDetailsPage(FoodItem foodItem)
        {
            refresh = false;
            var parameters = new Dictionary<string, object>
            {
                [nameof(DetailsViewModel.FoodItem)] = foodItem
            };
            await Shell.Current.GoToAsync(nameof(DetailPage), animate: true, parameters);
        }

        [RelayCommand]
        private async Task GoToCartViewPage()
        {
            await Shell.Current.GoToAsync(nameof(CartPage), animate: true);
        }
    }
}
