using DartsStatsApp.ViewModels;

namespace DartsStatsApp.Views;

public partial class TournamentDetailsView : ContentPage
{
	private TournamentDetailsViewModel _viewModel;
	public TournamentDetailsView(TournamentDetailsViewModel tournamentsVm)
	{
		InitializeComponent();
        _viewModel = tournamentsVm;
		BindingContext = _viewModel;
    }
}