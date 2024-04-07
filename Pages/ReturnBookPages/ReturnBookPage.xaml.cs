using QRCoder;

namespace Library;

public partial class ReturnBookPage : ContentPage
{
	public ReturnBookPage()
	{
		InitializeComponent();
	}
	private void RentBookQRClicked(object sender, EventArgs e)
	{
		INavigation navigation = ((Button)sender).Navigation;
		navigation.PushAsync(new ReturnBookQR());
	}
	private void RentBookManualClicked(object sender, EventArgs e)
	{
		INavigation navigation = ((Button)sender).Navigation;
		navigation.PushAsync(new ReturnBookManual());
	}
}