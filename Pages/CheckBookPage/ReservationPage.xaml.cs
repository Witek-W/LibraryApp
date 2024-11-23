using Library.Model;
using Plugin.NFC;

namespace Library.Pages.CheckBookPage;

public partial class ReservationPage : ContentPage
{
	private LibraryDbContext _context;
	private int IDbook = 0;
	private int ReaderID;
	private Books book;
	private Helpers _help;
	//NFC
	private readonly NFC _nfc;
	private bool NfcEnabled = false;
	private int ID = 0;
	public ReservationPage(int ID, LibraryDbContext context)
	{
		_context = context;
		InitializeComponent();
		_nfc = new NFC(Navigation);
		_help = new Helpers(Navigation);
		ButtonReservation.IsEnabled = false;
		IDbook = ID;
		try
		{
			book = _context.Book.FirstOrDefault(p => p.Id == IDbook);
		} catch
		{
			_help.ShowInternetError();
		}
		BookName.Text = "Tytu³ ksi¹¿ki: " + $"{book.Name}";
		Author.Text = "Autor ksi¹¿ki: " + $"{book.Author}";
		Type.Text = "Gatunek ksi¹¿ki: " + $"{book.Type}";
		ReturnDate.Text = "Planowana data zwrotu: " + $"{book.Planned_return_date.Value.Date.ToString("dd-MM-yyyy")}";
		if (!CrossNFC.Current.IsAvailable)
		{
			ReaderEntry.Text = "NFC jest niedostêpne";
		}
		else if (!CrossNFC.Current.IsEnabled)
		{
			ReaderEntry.Text = "NFC jest wy³¹czone";
		}
		else
		{
			NfcEnabled = true;
			ReaderEntry.Text = "Przy³ó¿ kartê czytelnika";
			_nfc.ReadNfcTag();
		}
		_nfc.MessageReceived += NFC_MessageReceived;
		CrossNFC.Current.OnNfcStatusChanged += Current_OnNfcStatusChanged;
	}
	async private void ReservationSend(object sender, EventArgs e)
	{
		ReaderID = Convert.ToInt32(ReaderEntry.Text);
		try
		{
			if (book.Reservation == 1)
			{
				await Application.Current.MainPage.DisplayAlert("Ostrze¿enie", "Ksi¹¿ka jest ju¿ zarezerwowana", "WyjdŸ");
			}
			else
			{
				if(book.ReaderID == ReaderID)
				{
					await Application.Current.MainPage.DisplayAlert("Ostrze¿enie", "Nie mo¿esz zarezerwowaæ wypo¿yczonej przez siebie ksi¹¿ki", "WyjdŸ");
					return;
				}
				book.Reservation = 1;
				book.ReservationReaderID = ReaderID;
				_context.SaveChanges();
				await Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Ksi¹¿ka zosta³a zarezerwowana", "Ok");
			}
			MainPage refreshMainPage = new MainPage();
			NavigationPage.SetHasBackButton(refreshMainPage, false);
			Navigation.PushAsync(refreshMainPage);
		}
		catch
		{
			_help.ShowInternetError();
		}
	}
	//NFC Reading
	private void Current_OnNfcStatusChanged(bool isEnabled)
	{
		try
		{
			ID = Convert.ToInt32(ReaderEntry.Text);
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
			ReaderEntry.Text = "Przy³ó¿ kartê czytelnika";
		}
		else
		{
			_nfc.StopReadNfcTag();
			ReaderEntry.Text = "NFC jest wy³¹czone";
		}
	}
	private void NFC_MessageReceived(object sender, string message)
	{
		Device.BeginInvokeOnMainThread(() =>
		{
			ReaderEntry.Text = message;
			ButtonReservation.IsEnabled = true;
			_nfc.StopReadNfcTag();
		});
	}
}