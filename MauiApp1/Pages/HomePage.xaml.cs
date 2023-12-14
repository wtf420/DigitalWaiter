using System.Diagnostics;

namespace MauiApp1.Pages;

public partial class HomePage : ContentPage
{
	private HomeViewModel _homeViewModel;
	public HomePage(HomeViewModel homeViewModel)
	{
		InitializeComponent();
		_homeViewModel = homeViewModel;
		BindingContext = _homeViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		_homeViewModel.Update();
	}

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        _homeViewModel.Update();
    }
}