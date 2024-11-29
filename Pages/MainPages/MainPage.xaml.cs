using Library.Model;
using Library.Pages.MainPages;
using Library.Pages.ManageLibraryPages;
using Library.Pages.Popups;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls;

namespace Library
{
	public class BookWithReaderInfo
	{
		public int ID { get; set; }
		public string BookName { get; set; }
		public string BookAuthor { get; set; }
		public string ReaderName { get; set; }
		public string ReaderSurname { get; set; }
		public string Planned_return { get; set; }

		public string Phone_Number { get; set; }
	}
	public partial class MainPage : ContentPage
	{
		private readonly LibraryDbContext _context;
		private List<BookWithReaderInfo> notifications;
		private Loading load;
		private ApiSms _sms;
		private Helpers _help;
		private bool smsApiFail = false;
		public MainPage()
		{
			_context = new LibraryDbContext();
			_sms = new ApiSms();
			_help = new Helpers(Navigation);
			var network = Connectivity.Current.NetworkAccess;
			Application.Current.UserAppTheme = AppTheme.Light;
			InitializeComponent();
			refreshanimate.IsAnimationEnabled = false;
			IconsLayout.IsVisible = true;
			bellanimation.IsVisible = false;
			bellstatic.IsVisible = true;
            //Wysyłanie SMS
			SendingSms();
		}
		private async void SendingSms()
		{
			List<BookWithReaderInfo> notificationss;
			try
			{
				notificationss = await _context.Book.Where(p => p.Planned_return_date < DateTime.Now && p.SmsSendApi == 0).Join(_context.Readers,
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
			}
            catch
            {
                _help.ShowInternetError();
                return;
            }
			smsApiFail = false;
			int apiNumber = 1;
			foreach (var not in notificationss)
			{
				string number = "48" + not.Phone_Number;
				string message = $"Dzień Dobry. Niezwłocznie prosimy Pana/Panią o oddanie zaległej książki pod tytułem: {not.BookName}. " +
									$"Książka miała zostać zwrócona dnia: {not.Planned_return}.";
				try
				{
					await _sms.SendSmsToReader(message, number);
				} catch
				{
					smsApiFail = true;
				}
				if(smsApiFail)
				{
					apiNumber = 2;
				}
				var bookToUpdate = _context.Book.FirstOrDefault(p => p.Id == not.ID);
				if (bookToUpdate != null)
				{
					bookToUpdate.SmsSendApi = apiNumber;
				}

			}
			await _context.SaveChangesAsync();
			if (smsApiFail) _help.ShowSMSApiError();
		}
		private void CheckBookPageButton(object sender, EventArgs e)
		{
			INavigation navigation = ((Button)sender).Navigation;
			navigation.PushAsync(new CheckBookPage());
		}
		private void RentBookPageButton(object sender, EventArgs e)
		{
			INavigation navigation = ((Button)sender).Navigation;
			navigation.PushAsync(new RentBookPage());
		}
		private void ReturnBookPageButton(object sender, EventArgs e)
		{
			INavigation navigation = ((Button)sender).Navigation;
			navigation.PushAsync(new ReturnBookPage());
		}
		private void AddBook(object sender, EventArgs e)
		{
			INavigation navigation = ((Button)sender).Navigation;
			navigation.PushAsync(new AddBookPage());
		}

		private void AddReader(object sender, EventArgs e)
		{
			INavigation navigation = ((Button)sender).Navigation;
			navigation.PushAsync(new AddReaderPage());
		}
		private void NotificationButton(object sender, EventArgs e)
		{
			NotificationPage resultsPage = new NotificationPage(_context);
			Navigation.PushAsync(resultsPage);
		}
		private void QRButton(object sender, EventArgs e)
		{
			QRMainPage resultsPage = new QRMainPage();
			Navigation.PushAsync(resultsPage);
		}
		private void IDReaderButton(object sender, EventArgs e)
		{
			ReaderChoice resultsPage = new ReaderChoice();
			Navigation.PushAsync(resultsPage);
		}
		
		private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			refreshanimate.IsAnimationEnabled = true;
			await Task.Delay(950);
			refreshanimate.IsAnimationEnabled = false;
			MainPage refreshMainPage = new MainPage();
			NavigationPage.SetHasBackButton(refreshMainPage, false);
			await Navigation.PushAsync(refreshMainPage);
		}
	}

}
