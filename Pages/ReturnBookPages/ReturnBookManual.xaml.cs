using Library.Pages.ReturnBookPages;

namespace Library;

public partial class ReturnBookManual : ContentPage
{
	private string qrnumber;
	public ReturnBookManual()
	{
		InitializeComponent();
		ButtonReturn.IsEnabled = false;
	}

	private void ReturnBookManualButton(object sender, EventArgs e)
	{
		qrnumber = ManualBookNumber.Text;
		ReturnBookQRConfirm resultsPage = new ReturnBookQRConfirm(qrnumber);
		Navigation.PushAsync(resultsPage);
	}
	private void ReturnLengthText(object sender, EventArgs e)
	{
		qrnumber = ManualBookNumber.Text;
		if (!string.IsNullOrEmpty(qrnumber))
		{
			ButtonReturn.IsEnabled = true;
		} else
		{
			ButtonReturn.IsEnabled = false;
		}
	}

}