using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    [QueryProperty(nameof(FromSearch), nameof(FromSearch))]
    public partial class AllPizzasViewModel: ObservableObject
    {
        private readonly FoodService _foodService;
        public AllPizzasViewModel(FoodService foodService)
        {
            _foodService = foodService;
            FoodItems = new(_foodService.GetAllFoodItems());
        }
        public ObservableCollection<FoodItem> FoodItems { get; set; }

        [ObservableProperty]
        private bool _fromSearch;

        [ObservableProperty]
        private bool _searching;

        [RelayCommand]
        private async Task SearchPizzas(string searchTerm)
        {
            FoodItems.Clear();
            Searching = true;
            foreach (var foodItem in _foodService.SearchFoodItems(searchTerm))
            {
                FoodItems.Add(foodItem);
            }
            Searching = false;
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
