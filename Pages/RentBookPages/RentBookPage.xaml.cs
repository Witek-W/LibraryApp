using ZXing.Net.Maui;
using Library.Pages.RentBookPages;
using Plugin.NFC;
using System.Text;
using Library.Model;

namespace Library;

public partial class RentBookPage : ContentPage
{
	private readonly NFC _nfc;
	private readonly Helpers _help;
	private bool NfcEnabled = false;
	private int ID = 0;
	private bool IDBookValid = true;
	public RentBookPage()
	{
		InitializeComponent();
		_nfc = new NFC(Navigation);
		_help = new Helpers(Navigation);
		qrbutton.IsVisible = true;
		labelhide.IsVisible = false;
		framehide.IsVisible = false;
		entryhide.IsVisible = false;
		frame2hide.IsVisible = false;
		imagehide.IsVisible = false;
		button2hide.IsVisible = false;
		if(!CrossNFC.Current.IsAvailable)
		{
			IDReaderInput.Text = "NFC jest niedostêpne";
		} else if(!CrossNFC.Current.IsEnabled)
		{
			IDReaderInput.Text = "NFC jest wy³¹czone";
		} else
		{
			NfcEnabled = true;
			IDReaderInput.Text = "Przy³ó¿ kartê czytelnika";
			_nfc.ReadNfcTag();
		}
		_nfc.MessageReceived += NFC_MessageReceived;
		CrossNFC.Current.OnNfcStatusChanged += Current_OnNfcStatusChanged;
	}
	private void Current_OnNfcStatusChanged(bool isEnabled)
	{
		try
		{
			ID = Convert.ToInt32(IDReaderInput.Text);
		} catch(Exception ex)
		{

		}
		if(ID > 0)
		{
			return;
		}
		if(isEnabled)
		{
			_nfc.ReadNfcTag();
			IDReaderInput.Text = "Przy³ó¿ kartê czytelnika";
		} else
		{
			_nfc.StopReadNfcTag();
			IDReaderInput.Text = "NFC jest wy³¹czone";
		}
	}
	private void NFC_MessageReceived(object sender, string message)
	{
		Device.BeginInvokeOnMainThread(() =>
		{
			IDReaderInput.Text = message;
			qrbutton.IsEnabled = true;
			_nfc.StopReadNfcTag();
		});
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
	private void entryhidechanged(object sender, EventArgs e)
	{
		//Walidacja ID ksiazki
		if(_help.CheckString(entryhide.Text,4))
		{
			_help.ChangeUI(labelhide, framehide, frame2hide, imagehide, false, "bookid.png");
			IDBookValid = false;
		} else
		{
			_help.ChangeUI(labelhide, framehide, frame2hide, imagehide, true, "bookid.png");
			IDBookValid = true;
		}

		if(!string.IsNullOrEmpty(entryhide.Text) && IDBookValid)
		{
			button2hide.IsEnabled = true;
		} else
		{
			button2hide.IsEnabled = false;
		}
	}
}