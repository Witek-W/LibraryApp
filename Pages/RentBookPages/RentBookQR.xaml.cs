using Library.Pages.RentBookPages;
using ZXing.Net.Maui;

namespace Library;

public partial class RentBookQR : ContentPage
{
	string result;
	string idclient;
	public RentBookQR(string id)
	{
		InitializeComponent();
		idclient = id;
	}
	protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
	{
		MainThread.BeginInvokeOnMainThread(() =>
		{
			result = $"{e.Results[0].Value}";
			barcodeView.IsDetecting = false;
			RentBookQRConfirm resultsPage = new RentBookQRConfirm(result, idclient);
			Navigation.PushAsync(resultsPage);
		});
	}

}