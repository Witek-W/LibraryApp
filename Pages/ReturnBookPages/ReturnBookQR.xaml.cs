using Library.Pages.ReturnBookPages;
using ZXing.Net.Maui;

namespace Library;

public partial class ReturnBookQR : ContentPage
{
	string result;
	public ReturnBookQR()
	{
		InitializeComponent();
	}
	protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
	{
		MainThread.BeginInvokeOnMainThread(() =>
		{
			result = $"{e.Results[0].Value}";
			barcodeView.IsDetecting = false;
			ReturnBookQRConfirm resultsPage = new ReturnBookQRConfirm(result);
			Navigation.PushAsync(resultsPage);
		});
	}
}