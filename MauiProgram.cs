using epj.Expander.Maui;
using Library.ViewModel;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using ZXing.Net.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;


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
				.UseSkiaSharp()
				.ConfigureFonts(fonts =>
                {
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

			Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (h, v) =>
			{
				h.PlatformView.BackgroundTintList =
					Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
			});


			builder.Services.AddSingleton<MainPage>();
			builder.Services.AddSingleton<MainViewModel>();
			builder.Services.AddTransient<CheckBookPage>();
			builder.Services.AddTransient<CheckBookViewModel>();
            builder.Services.AddTransient<RentBookPage>();
            builder.Services.AddTransient<RentBookViewModel>();
            builder.Services.AddTransient<ReturnBookPage>();
            builder.Services.AddTransient<ReturnBookViewModel>();
            builder.Services.AddTransient<ManageLibraryViewModel>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
			Expander.EnableAnimations();
			return builder.Build();
		}
	}
}
