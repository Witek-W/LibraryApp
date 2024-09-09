using Library.Model;
using ZXing.Net.Maui;

namespace Library.Pages.MainPages;

public partial class QRMainPage : ContentPage
{
	string result = "";
	public QRMainPage()
	{
		InitializeComponent();
	}
	protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
	{
		MainThread.BeginInvokeOnMainThread(async () =>
		{
			result = $"{e.Results[0].Value}";
			qrScanner.IsDetecting = false;
			int number = 0;
			try
			{
				number = Convert.ToInt32(result);
			}
			catch
			{
				MainPage main = new MainPage();
				NavigationPage.SetHasBackButton(main, false);
				await Navigation.PushAsync(main);
				return;
			}
			ChoiceBook resultsPage = new ChoiceBook(number);
			Navigation.PushAsync(resultsPage);
		});
	}
}