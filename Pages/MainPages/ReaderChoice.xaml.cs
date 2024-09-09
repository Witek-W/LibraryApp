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
	public ReaderChoice()
	{
		InitializeComponent();
		_nfc = new NFC(Navigation);
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
	private void EditButtonClick(object sender, EventArgs e)
	{
		EditReader edit = new EditReader(ID);
		Navigation.PushAsync(edit);
	}
	private async void DeleteButtonClick(object sender, EventArgs e)
	{
		await Delete();
		MainPage Main = new MainPage();
		NavigationPage.SetHasBackButton(Main, false);
		Navigation.PushAsync(Main);
	}
	private async Task Delete()
	{
		var ReaderToDelete = await _context.Readers.FirstOrDefaultAsync(p => p.Id == ID);
		if (ReaderToDelete != null)
		{
			_context.Readers.Remove(ReaderToDelete);
			await _context.SaveChangesAsync();
		}
	}
}