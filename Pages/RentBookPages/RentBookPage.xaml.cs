using ZXing.Net.Maui;
using Library.Pages.RentBookPages;

namespace Library;

public partial class RentBookPage : ContentPage
{
	public RentBookPage()
	{
		InitializeComponent();
		qrbutton.IsVisible = true;
		labelhide.IsVisible = false;
		framehide.IsVisible = false;
		entryhide.IsVisible = false;
		frame2hide.IsVisible = false;
		imagehide.IsVisible = false;
		button2hide.IsVisible = false;
	}
	private void InputIsHere(object sender, EventArgs e)
	{
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
		string idbook = entryhide.Text;
		RentBookQRConfirm resultsPage = new RentBookQRConfirm(idbook, id);
		Navigation.PushAsync(resultsPage);
	}
	private void OnSwitchToggled(object sender, ToggledEventArgs e)
	{
		if(e.Value == true)
		{
			qrbutton.IsVisible = false;
			labelhide.IsVisible = true;
			framehide.IsVisible = true;
			entryhide.IsVisible = true;
			frame2hide.IsVisible = true;
			imagehide.IsVisible = true;
			button2hide.IsVisible = true;

		} else
		{
			qrbutton.IsVisible = true;
			labelhide.IsVisible = false;
			framehide.IsVisible = false;
			entryhide.IsVisible = false;
			frame2hide.IsVisible = false;
			imagehide.IsVisible = false;
			button2hide.IsVisible = false;
		}
	}
}