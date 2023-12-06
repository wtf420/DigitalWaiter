namespace MauiApp1.Pages;

public partial class AllPizzasPage : ContentPage
{
	private readonly AllPizzasViewModel _allPizzasViewModel;
	public AllPizzasPage(AllPizzasViewModel allPizzasViewModel)
	{
		InitializeComponent();
		_allPizzasViewModel = allPizzasViewModel;
		BindingContext = _allPizzasViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		if (_allPizzasViewModel.FromSearch)
		{
			searchBar.Focus();
		}
    }

    private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
		if (!string.IsNullOrWhiteSpace(e.OldTextValue) && string.IsNullOrWhiteSpace(e.NewTextValue))
		{
			_allPizzasViewModel.SearchPizzasCommand.Execute(null);
		}
    }
}