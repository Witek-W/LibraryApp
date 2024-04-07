using ZXing.Net.Maui;

namespace Library;

public partial class RentBookPage : ContentPage
{
	public RentBookPage()
	{
		InitializeComponent();
	}
	
	private void RentBookQRClicked(object sender, EventArgs e)
	{
		INavigation navigation = ((Button)sender).Navigation;
		navigation.PushAsync(new RentBookQR());
	}
	private void RentBookManualClicked(object sender, EventArgs e)
	{
		INavigation navigation = ((Button)sender).Navigation;
		navigation.PushAsync(new RentBookManual());
	}
}