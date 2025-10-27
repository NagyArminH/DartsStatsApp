using DartsStatsApp.ViewModels;

namespace DartsStatsApp.Views;

public partial class HomePageView : ContentPage
{
	private HomePageViewModel _viewModel;
	public HomePageView(HomePageViewModel vm)
	{
		InitializeComponent();
        _viewModel = vm;
		BindingContext = _viewModel;
    }
}