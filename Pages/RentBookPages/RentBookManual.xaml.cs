using Library.Pages.RentBookPages;

namespace Library;

public partial class RentBookManual : ContentPage
{
	private string qrnumber;
	private string clientid;
	public RentBookManual(string qr)
	{
		InitializeComponent();
		RentButton.IsEnabled = false;
		clientid = qr;
	}
	private void RentBookManualButton(object sender, EventArgs e)
	{
		qrnumber = ManualBookNumber.Text;
		RentBookQRConfirm resultsPage = new RentBookQRConfirm(qrnumber, clientid);
		Navigation.PushAsync(resultsPage);
	}
	private void RentChanged(object sender, EventArgs e)
	{
		qrnumber = ManualBookNumber.Text;
		if(!string.IsNullOrEmpty(qrnumber) && !string.IsNullOrEmpty(clientid))
		{
			RentButton.IsEnabled = true;
		} else
		{
			RentButton.IsEnabled = false;
		}
	}
}