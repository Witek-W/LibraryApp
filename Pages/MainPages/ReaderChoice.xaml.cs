using Android.OS;
using Library.Model;
using Microsoft.EntityFrameworkCore;
using Plugin.NFC;

namespace Library.Pages.MainPages;

public partial class ReaderChoice : ContentPage
{
	private readonly NFC _nfc;
	private readonly LibraryDbContext _context;
	private bool NfcEnabled = false;
	private int ID = 0;
	private Helpers _help;
	public ReaderChoice()
	{
		InitializeComponent();
		_nfc = new NFC(Navigation);
		_help = new Helpers(Navigation);
		_context = new LibraryDbContext();
		if (!CrossNFC.Current.IsAvailable)
		{
			IDReaderInput.Text = "NFC jest niedostêpne";
		}
		else if (!CrossNFC.Current.IsEnabled)
		{
			IDReaderInput.Text = "NFC jest wy³¹czone";
		}
		else
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
		}
		catch (Exception ex)
		{

		}
		if (ID > 0)
		{
			return;
		}
		if (isEnabled)
		{
			_nfc.ReadNfcTag();
			IDReaderInput.Text = "Przy³ó¿ kartê czytelnika";
		}
		else
		{
			_nfc.StopReadNfcTag();
			IDReaderInput.Text = "NFC jest wy³¹czone";
		}
	}

	[Obsolete]
	private void NFC_MessageReceived(object sender, string message)
	{
		Device.BeginInvokeOnMainThread(() =>
		{
			IDReaderInput.Text = message;
			ID = Convert.ToInt32(message);
			EditButton.IsEnabled = true;
			DeleteButton.IsEnabled = true;
			_nfc.StopReadNfcTag();
		});
	}
	private async void EditButtonClick(object sender, EventArgs e)
	{
		var user = await _context.Readers.FirstOrDefaultAsync(p => p.Id == ID);
		if(user != null)
		{
			EditReader edit = new EditReader(user);
			Navigation.PushAsync(edit);
		} else
		{
			_help.ShowSQLError();
		}
	}
	private async void DeleteButtonClick(object sender, EventArgs e)
	{
		var ReaderToDelete = await _context.Readers.FirstOrDefaultAsync(p => p.Id == ID);
		if (ReaderToDelete != null)
		{
			_context.Readers.Remove(ReaderToDelete);
			await _context.SaveChangesAsync();
			MainPage Main = new MainPage();
			NavigationPage.SetHasBackButton(Main, false);
			Navigation.PushAsync(Main);
		}
		else
		{
			_help.ShowSQLError();
		}
	}
}