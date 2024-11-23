using Library.Model;

namespace Library.Pages.ReturnBookPages;

public partial class ReturnBookQRConfirm : ContentPage
{
	private readonly LibraryDbContext _context;
	private int qr;
	private Books result;
	private int ReservationBookLength = 2;
	private Helpers _help;
	private string PhoneNumerReader = "";
	private ApiSms _sms;
	public ReturnBookQRConfirm(string qrresult)
	{
		InitializeComponent();
		_context = new LibraryDbContext();
		qr = Convert.ToInt32(qrresult);
		_sms = new ApiSms();
		_help = new Helpers(Navigation);
		try
		{
			result = _context.Book.FirstOrDefault(p => p.Id == qr);
			if(result == null)
			{
				_help.ShowSQLError();
				return;
			}
			ManualResultName.Text = "Tytu≥ ksiπøki: " + $"{result.Name}";
			ManualResultAuthor.Text = "Autor ksiπøki: " + $"{result.Author}";
			ManualResultType.Text = "Gatunek ksiπøki: " + $"{result.Type}";
			if(result.Reservation == 1)
			{
				var reader = _context.Readers.FirstOrDefault(p => p.Id == result.ReservationReaderID);
				PhoneNumerReader = reader.Phone_Number;
			}
		}
		catch
		{
			_help.ShowInternetError();
		}
	}
	private void ReturnToMainPage(object sender, EventArgs e)
	{
		MainPage BrandNew = new MainPage();
		NavigationPage.SetHasBackButton(BrandNew, false);
		Navigation.PushAsync(BrandNew);
	}
	private async void ReturnBookClicked(object sender, EventArgs e)
	{
		if(result.Availability == 0)
		{
			try
			{
				result.Availability = 1;
				result.Rental_date = null;
				result.Planned_return_date = null;
				result.ReaderID = null;
				result.SmsSendApi = 0;
				if (result.Reservation == 1)
				{
					DateTime dateTime = DateTime.Now;
					dateTime = dateTime.AddDays(ReservationBookLength);
					result.Reservation_End = dateTime;
					string number = "48" + PhoneNumerReader;
					string message = $"Dzien dobry. Informujemy, ze zarezerwowana ksiazka pod tytulem: {result.Name} jest juø dostepna do wypozyczenia.";
					try
					{
						_sms.SendSmsToReader(message, number);
					} catch
					{
						_context.SaveChanges();
						_help.ShowSMSApiError();
					}
				}
				_context.SaveChanges();
				await DisplayAlert("Powiadomienie", "Ksiπøka zosta≥a zwrÛcona", "Wyjdü");
			}
			catch
			{
				_help.ShowInternetError();
			}
		} else
		{
			await DisplayAlert("Ostrzeøenie", "Ksiπøka nie jest wypoøyczona", "Wyjdü");
		}
		MainPage BrandNew = new MainPage();
		NavigationPage.SetHasBackButton(BrandNew, false);
		Navigation.PushAsync(BrandNew);
	}
}