using Library.Pages.ManageLibraryPages;

namespace Library;

public partial class ManageLibraryPage : ContentPage
{
	public ManageLibraryPage()
	{
		InitializeComponent();
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
}