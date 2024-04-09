using Library.Model;
using Library.Pages.MainPages;
using Library.ViewModel;

namespace Library
{
	public partial class MainPage : ContentPage
	{
		private readonly LibraryDbContext _context;
		private IQueryable<Books> notifications;
		public MainPage()
		{
			InitializeComponent();
			_context = new LibraryDbContext();
			DateTime today = DateTime.Now;
			notifications = _context.Book.Where(p => p.Planned_return_date < today);
			NotificationsView.Text = notifications.Count().ToString();
			if(notifications.Count() > 0)
			{
				FrameNotification.IsVisible = true;
			} else
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
