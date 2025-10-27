using DartsStatsApp.ViewModels;

namespace DartsStatsApp.Views;

public partial class PlayerProfileView : ContentPage
{
	private PlayerProfileViewModel _viewModel;
	public PlayerProfileView(PlayerProfileViewModel vm)
	{
		InitializeComponent();
		_viewModel = vm;
		BindingContext = _viewModel;
	}
}