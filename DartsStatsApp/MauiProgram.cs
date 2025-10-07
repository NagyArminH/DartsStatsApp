using DartsStatsApp.Services;
using DartsStatsApp.ViewModels;
using DartsStatsApp.Views;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace DartsStatsApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<DbService>();

		builder.Services.AddTransient<OOMListView>();
        builder.Services.AddTransient<TournamentsView>();
        builder.Services.AddTransient<TournamentDetailsView>();

        builder.Services.AddTransient<OOMListViewModel>();
		builder.Services.AddTransient<TournamentsViewModel>();
        builder.Services.AddTransient<TournamentDetailsViewModel>();

        return builder.Build();
	}
}
