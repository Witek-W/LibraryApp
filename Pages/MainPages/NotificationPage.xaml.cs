using Library.Model;
using System.Linq;
using System.Xml;

namespace Library.Pages.MainPages;

public partial class NotificationPage : ContentPage
{
	private LibraryDbContext _context;
	private Helpers _help;
	public NotificationPage(IQueryable<BookWithReaderInfo> results, LibraryDbContext context)
	{
		InitializeComponent();
		ResultsListView.ItemsSource = results.ToList();
		_context = context;
		_help = new Helpers(Navigation);
	}

	 async void SendSms(object sender, EventArgs e)
	{
		Button button = (Button)sender;
		var book = button.BindingContext as BookWithReaderInfo;
		int ID = book.ID;
		Books result = null;
		try
		{
			result = _context.Book.FirstOrDefault(p => p.Id == ID);
		}
		catch
		{
			_help.ShowInternetError();	
		}

		int? ReaderID = result.ReaderID;
		var resultReader = _context.Readers.FirstOrDefault(p => p.Id == ReaderID);

		string message = $"Dzie� Dobry. Niezw�ocznie prosimy Pana/Pani� o oddanie zaleg�ej ksi��ki pod tytu�em: {result.Name}. Ksi��ka mia�a zosta� zwr�cona dnia: {result.Planned_return_date.Value.Date.ToString("dd-MM-yyyy")}.";

		await SmsMethod(message, resultReader.Phone_Number);
	}
	async Task SmsMethod(string messageText, string number)
	{
		var message = new SmsMessage(messageText, number);
		await Sms.ComposeAsync(message);
	
	}
}