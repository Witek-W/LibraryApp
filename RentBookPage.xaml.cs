using ZXing.Net.Maui;

namespace Library;

public partial class RentBookPage : ContentPage
{
	public RentBookPage()
	{
		InitializeComponent();
	}
	protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
	{
		MainThread.BeginInvokeOnMainThread(() =>
		{
			lbl.Text = $"{e.Results[0].Value}";
            barcodeView.IsDetecting = false;
        });
	}
}