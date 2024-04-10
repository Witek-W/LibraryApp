using epj.Expander.Maui;
using Library.ViewModel;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;


namespace Library
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
                .UseBarcodeReader()
				.UseExpander()
				.ConfigureFonts(fonts =>
                {
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

			builder.Services.AddSingleton<MainPage>();
			builder.Services.AddSingleton<MainViewModel>();
			builder.Services.AddTransient<CheckBookPage>();
			builder.Services.AddTransient<CheckBookViewModel>();
            builder.Services.AddTransient<RentBookPage>();
            builder.Services.AddTransient<RentBookViewModel>();
            builder.Services.AddTransient<ReturnBookPage>();
            builder.Services.AddTransient<ReturnBookViewModel>();
            builder.Services.AddTransient<ManageLibraryPage>();
            builder.Services.AddTransient<ManageLibraryViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
			Expander.EnableAnimations();
			return builder.Build();
		}
	}
}
