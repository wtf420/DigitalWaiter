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
}