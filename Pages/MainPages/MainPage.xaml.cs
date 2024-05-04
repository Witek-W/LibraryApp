using Library.Model;
using Library.Pages.MainPages;
using Library.ViewModel;

namespace Library
{
	public class BookWithReaderInfo
	{
		public string BookName { get; set; }
		public string BookAuthor { get; set; }
		public string ReaderName { get; set; }
		public string ReaderSurname { get; set; }
		public string Planned_return { get; set; }
	}
	public partial class MainPage : ContentPage
	{
		private readonly LibraryDbContext _context;
		private IQueryable<BookWithReaderInfo> notifications;
		public MainPage()
		{
			InitializeComponent();
			_context = new LibraryDbContext();
			DateTime today = DateTime.Now;
			notifications = _context.Book.Where(p => p.Planned_return_date < today).Join(_context.Readers,
							book => book.ReaderID,
							Readers => Readers.Id,
							(Books, Readers) => new BookWithReaderInfo
							{
								BookName = Books.Name,
								BookAuthor = Books.Author,
								ReaderName = Readers.Name,
								ReaderSurname = Readers.Surname,
								Planned_return = Books.Planned_return_date.Value.Date.ToString("dd-MM-yyyy")
							});
			if (notifications.Count() > 0 && notifications.Count() < 99)
			{
				NotificationsView.Text = notifications.Count().ToString();
				FrameNotification.IsVisible = true;

			} else if(notifications.Count() >= 99) {
				NotificationsView.Text = "99";
				FrameNotification.IsVisible = true;
			}
			else
			{
				FrameNotification.IsVisible = false;
			}
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
		private void ManageLibraryPageButton(object sender, EventArgs e)
		{
			INavigation navigation = ((Button)sender).Navigation;
			navigation.PushAsync(new ManageLibraryPage());
		}
		private void NotificationButton(object sender, EventArgs e)
		{
			NotificationPage resultsPage = new NotificationPage(notifications);
			Navigation.PushAsync(resultsPage);
		}
		private void RefreshButton(object sender, EventArgs e)
		{
			MainPage refreshMainPage = new MainPage();
			NavigationPage.SetHasBackButton(refreshMainPage, false);
			Navigation.PushAsync(refreshMainPage);
		}
	}

}
