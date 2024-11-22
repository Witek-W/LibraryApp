using Library.Model;
using Library.Pages.ReturnBookPages;

namespace Library;

public partial class ReturnBookManual : ContentPage
{
	private string qrnumber;
	private readonly Helpers _help;
	private bool IDBookValid = true;
	public ReturnBookManual()
	{
		InitializeComponent();
		_help = new Helpers(Navigation);
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
		//Walidacja ID ksiazki
		if(_help.CheckString(ManualBookNumber.Text,4))
		{
			_help.ChangeUI(LabelManual, FrameManual, FrameBackgroundManual, ImageManual, false, "readerid.png");
			IDBookValid = false;
		} else
		{
			_help.ChangeUI(LabelManual, FrameManual, FrameBackgroundManual, ImageManual, true, "readerid.png");
			IDBookValid = true;
		}
		if (!string.IsNullOrEmpty(ManualBookNumber.Text) && IDBookValid)
		{
			ButtonReturn.IsEnabled = true;
		}
		else
		{
			ButtonReturn.IsEnabled = false;
		}
	}

}