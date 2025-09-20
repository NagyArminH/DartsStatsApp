using DartsStatsApp.Services;
using DartsStatsApp.ViewModels;
using DartsStatsApp.Views;
using Microsoft.Extensions.Logging;

namespace DartsStatsApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
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
        builder.Services.AddTransient<OOMListViewModel>();

        builder.Services.AddTransient(typeof(OOMListView));
		return builder.Build();
	}
}
