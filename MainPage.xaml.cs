using Library.ViewModel;

namespace Library
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

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
			navigation.PushAsync(new AddBookPage());
		}
	}

}
