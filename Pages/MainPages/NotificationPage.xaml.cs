using Library.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Xml;

namespace Library.Pages.MainPages;

public partial class NotificationPage : ContentPage
{
	private LibraryDbContext _context;
	private Helpers _help;
	private ApiSms _sms;
	private bool apiSmsFailed = false;
	public NotificationPage(LibraryDbContext context)
	{
		_context = context;
		_help = new Helpers(Navigation);
		_sms = new ApiSms();
		InitializeComponent();
		LoadData();
	}
	private async void LoadData()
	{
		try
		{
			var results = await _context.Book.Where(p => p.Planned_return_date < DateTime.Now && p.SmsSendApi == 2).Join(_context.Readers,
						   book => book.ReaderID,
						   Readers => Readers.Id,
						   (Books, Readers) => new BookWithReaderInfo
						   {
							   ID = Books.Id,
							   BookName = Books.Name,
							   BookAuthor = Books.Author,
							   ReaderName = Readers.Name,
							   ReaderSurname = Readers.Surname,
							   Planned_return = Books.Planned_return_date.Value.Date.ToString("dd-MM-yyyy"),
							   Phone_Number = Readers.Phone_Number
						   }).ToListAsync();
			ResultsListView.ItemsSource = results;
			if(results.Count() == 0)
			{
				NoUsersLabel.IsVisible = true;
				ResultsListView.IsVisible = false;
			} else
			{
				NoUsersLabel.IsVisible = false;
				ResultsListView.IsVisible = true;
			}

		}
		catch
		{
			_help.ShowInternetError();
		}
	}

	async void SendSms(object sender, EventArgs e)
	{
		apiSmsFailed = false;
		Button button = (Button)sender;
		var book = button.BindingContext as BookWithReaderInfo;
		int ID = book.ID;
		Books result = null;
		try
		{
			//Do zmiany?
			result = _context.Book.FirstOrDefault(p => p.Id == ID);
		}
		catch
		{
			_help.ShowInternetError();	
		}

		int? ReaderID = result.ReaderID;
		var resultReader = _context.Readers.FirstOrDefault(p => p.Id == ReaderID);

		string message = $"Dzien dobry. Niezwlocznie prosimy Pana/Pania o oddanie zaleglej ksiazki pod tytulem: {result.Name}. Ksiazka miala zostac zwrocona dnia: {result.Planned_return_date.Value.Date.ToString("dd-MM-yyyy")}.";

		try
		{
			await _sms.SendSmsToReader(message, resultReader.Phone_Number);
		}catch
		{
			apiSmsFailed = true;
		}
		if(apiSmsFailed)
		{
			_help.ShowSMSApiError();
		} else
		{
			result.SmsSendApi = 1;
			await _context.SaveChangesAsync();
			await Navigation.PopAsync();
		}
	}
}