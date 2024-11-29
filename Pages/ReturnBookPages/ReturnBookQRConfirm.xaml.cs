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
	private DateTime? plannedreturn;
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
			ManualResultName.Text = "Tytu³ ksi¹¿ki: " + $"{result.Name}";
			ManualResultAuthor.Text = "Autor ksi¹¿ki: " + $"{result.Author}";
			ManualResultType.Text = "Gatunek ksi¹¿ki: " + $"{result.Type}";
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
				plannedreturn = result.Planned_return_date;
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
					string message = $"Dzien dobry. Informujemy, ze zarezerwowana ksiazka pod tytulem: {result.Name} jest ju¿ dostepna do wypozyczenia.";
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
				DateTime now = DateTime.Now;
                if (plannedreturn.HasValue && plannedreturn < now)
				{
					TimeSpan delay = now - plannedreturn.Value;
					int delaydays = delay.Days;
                    await DisplayAlert("Powiadomienie", $"Ksi¹¿ka zosta³a zwrócona z {delaydays} dniowym opóŸnieniem", "WyjdŸ");
                } else
				{
                    await DisplayAlert("Powiadomienie", "Ksi¹¿ka zosta³a zwrócona", "WyjdŸ");
                }
			}
			catch
			{
				_help.ShowInternetError();
			}
		} else
		{
			await DisplayAlert("Ostrze¿enie", "Ksi¹¿ka nie jest wypo¿yczona", "WyjdŸ");
		}
		MainPage BrandNew = new MainPage();
		NavigationPage.SetHasBackButton(BrandNew, false);
		Navigation.PushAsync(BrandNew);
	}
}