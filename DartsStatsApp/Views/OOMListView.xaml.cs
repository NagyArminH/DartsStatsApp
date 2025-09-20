using DartsStatsApp.ViewModels;

namespace DartsStatsApp.Views;

public partial class OOMListView : ContentPage
{
	private OOMListViewModel _viewModel;
	public OOMListView(OOMListViewModel vm)
	{
		InitializeComponent();
		_viewModel = vm;
		BindingContext = _viewModel;
	}
}