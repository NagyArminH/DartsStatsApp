using DartsStatsApp.ViewModels;

namespace DartsStatsApp.Views;

public partial class PlayerComparisonView : ContentPage
{
	private PlayerComparisonViewModel _viewModel;
	public PlayerComparisonView(PlayerComparisonViewModel vm)
	{
		InitializeComponent();
        _viewModel = vm;
        BindingContext = _viewModel;
    }
}