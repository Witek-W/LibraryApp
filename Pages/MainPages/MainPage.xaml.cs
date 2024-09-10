using Library.Model;
using Library.Pages.MainPages;
using Library.Pages.ManageLibraryPages;
using Library.Pages.Popups;
using Microsoft.EntityFrameworkCore;

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
		private IQueryable<BookWithReaderInfo> notifications;
		private Loading load;
		private ApiSms _sms;
		public MainPage()
		{
			DateTime today = DateTime.Now;
			_context = new LibraryDbContext();
			_sms = new ApiSms();
			Application.Current.UserAppTheme = AppTheme.Light;
			try
			{
				notifications = _context.Book.Where(p => p.Planned_return_date < today && p.SmsSendApi == 0).Join(_context.Readers,
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
							});
			}
			catch (Exception ex)
			{
				ErrorStartPage BrandNew = new ErrorStartPage();
				NavigationPage.SetHasBackButton(BrandNew, false);
				Navigation.PushAsync(BrandNew);
			}
			//Sending sms
			foreach (var not in notifications)
			{
				string number = "48" + not.Phone_Number;
				string message = $"Dzien Dobry. Niezwlocznie prosimy Pana/Pania o oddanie zaleglej ksiazki pod tytulem: {not.BookName}. " +
									$"Ksiazka miala zostac zwrocona dnia: {not.Planned_return}.";
				try
				{
					_sms.SendSmsToReader(message, number);
				}
				catch
				{
					return;
				}

				using (var context = new LibraryDbContext())
				{
					var bookToUpdate = context.Book.FirstOrDefault(p => p.Id == not.ID);
					if (bookToUpdate != null)
					{
						bookToUpdate.SmsSendApi = 1;
						context.SaveChanges();
					}
				}
			}
			 

			InitializeComponent();
			refreshanimate.IsAnimationEnabled = false;
			if (notifications != null) {
				var res = notifications.Count();
				IconsLayout.IsVisible = true;
				if (res > 0 && res < 99)
				{
					NotificationsView.Text = notifications.Count().ToString();
					FrameNotification.IsVisible = true;
					bellanimation.IsVisible = true;
					bellstatic.IsVisible = false;

				}
				else if (res >= 99)
				{
					NotificationsView.Text = "99";
					FrameNotification.IsVisible = true;
					bellanimation.IsVisible = true;
					bellstatic.IsVisible = false;
				}
				else
				{
					FrameNotification.IsVisible = false;
					bellanimation.IsVisible = false;
					bellstatic.IsVisible = true;
				}
			}
		}
		private async Task SendingSms()
		{
			foreach (var not in notifications)
			{
				string number = "48" + not.Phone_Number;
				string message = $"Dzień Dobry. Niezwłocznie prosimy Pana/Panią o oddanie zaległej książki pod tytułem: {not.ReaderName}. " +
									$"Książka miała zostać zwrócona dnia: {not.Planned_return}.";
				//_sms.SendSmsToReader(message, number);
				var bookToUpdate = await _context.Book.FirstOrDefaultAsync(p => p.Id == not.ID);
				if (bookToUpdate != null)
				{
					bookToUpdate.SmsSendApi = 1;
				}
			}
			await _context.SaveChangesAsync();
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
			NotificationPage resultsPage = new NotificationPage(notifications, _context);
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
