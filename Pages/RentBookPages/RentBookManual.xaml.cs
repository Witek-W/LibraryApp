using Library.Pages.RentBookPages;

namespace Library;

public partial class RentBookManual : ContentPage
{
	public RentBookManual()
	{
		InitializeComponent();
	}
	private void RentBookManualButton(object sender, EventArgs e)
	{
		string qrnumber = ManualBookNumber.Text;
		RentBookQRConfirm resultsPage = new RentBookQRConfirm(qrnumber);
		Navigation.PushAsync(resultsPage);
	}
}