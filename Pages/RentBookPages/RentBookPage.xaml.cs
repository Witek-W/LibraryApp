using ZXing.Net.Maui;

namespace Library;

public partial class RentBookPage : ContentPage
{
	public RentBookPage()
	{
		InitializeComponent();
		Qrbutton.IsEnabled = false;
		ManualButton.IsEnabled = false;
	}
	private void InputIsHere(object sender, EventArgs e)
	{
		Qrbutton.IsEnabled = true;
		ManualButton.IsEnabled = true;
	}
	private void RentBookQRClicked(object sender, EventArgs e)
	{
		string id = IDReaderInput.Text;
		RentBookQR resultsPage = new RentBookQR(id);
		Navigation.PushAsync(resultsPage);
	}
	private void RentBookManualClicked(object sender, EventArgs e)
	{
		string id = IDReaderInput.Text;
		RentBookManual resultsPage = new RentBookManual(id);
		Navigation.PushAsync(resultsPage);
	}
}