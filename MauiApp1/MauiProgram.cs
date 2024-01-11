using CommunityToolkit.Maui;
using MauiApp1.Pages;
using MauiApp1.Services;
using MauiApp1.ViewModels;
using Microsoft.Extensions.Logging;
using Camera.MAUI;

namespace MauiApp1;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCameraView()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Urbanist-Italic-VariableFont-wght.ttf", "UrbanistItalic");
				fonts.AddFont("Urbanist-VariableFont-wght.ttf", "Urbanist");
				fonts.AddFont("Inter-Medium.ttf", "InterMedium");
				fonts.AddFont("Inter-Regular.ttf", "Inter");
				fonts.AddFont("Inter-Semibold.ttf", "InterSemiBold");
			})
			.UseMauiCommunityToolkit();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		AddPizzaServices(builder.Services);
        var app = builder.Build();
        ServiceHelper.Initialize(app.Services);
        return app;
	}

	private static IServiceCollection AddPizzaServices(IServiceCollection services)
	{
		services.AddSingleton<FoodService>();
        services.AddSingleton<OrderService>();
        services.AddSingleton<HomePage>().AddSingleton<HomeViewModel>();
		services.AddTransientWithShellRoute<AllPizzasPage, AllPizzasViewModel>(nameof(AllPizzasPage));
        services.AddTransientWithShellRoute<DetailPage, DetailsViewModel>(nameof(DetailPage));
		services.AddSingleton<CartViewModel>();
		services.AddTransient<CartPage>();
        return services;
	}
}
