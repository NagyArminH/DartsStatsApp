using DartsStatsApp.ViewModels;

namespace DartsStatsApp.Views;

public partial class TournamentsView : ContentPage
{
	private TournamentsViewModel _viewModel;
	public TournamentsView(TournamentsViewModel tournamentVm)
	{
		InitializeComponent();
		_viewModel = tournamentVm;
		BindingContext = _viewModel;
	}
}