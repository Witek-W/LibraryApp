using epj.Expander.Maui;
using Library.Model;
using Library.Pages.CheckBookPage;
using System.Globalization;

namespace Library;

public partial class SearchResultPage : ContentPage
{
	private int currentPage = 0;
	private const int pageSize = 9;
	private List<Model.Books> allbooks;
	private LibraryDbContext _context;
	public SearchResultPage(List<Model.Books> results, LibraryDbContext context)
	{
		InitializeComponent();
		allbooks = results;
		LoadPage(currentPage);
		PageNumberLabel.Text = (currentPage + 1).ToString();
		_context = context;
	}
	private void LoadPage(int pageNumber)
	{
		var booksForPage = allbooks.Skip(pageNumber * pageSize).Take(pageSize).ToList();
		ResultsListView.ItemsSource = booksForPage;
		currentPage = pageNumber;
	}
	public void NextPage(object sender, EventArgs e)
	{
		LoadPage(currentPage + 1);
		PageNumberLabel.Text = (currentPage + 1).ToString();
	}
	public void PreviousPage(object sender, EventArgs e)
	{
		if(currentPage > 0)
		{
			LoadPage(currentPage - 1);
			PageNumberLabel.Text = (currentPage + 1).ToString();
		}
	}

	private void TestExpander(object sender, EventArgs e)
	{
		var expander = (Expander)sender;
		expander.IsExpanded = !expander.IsExpanded;
	}
	async void BookDeleteButton(object sender, EventArgs e)
	{
		Button button = (Button)sender;
		var book = button.BindingContext as Model.Books;
		bool result = await Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usun¹æ tê ksi¹¿kê?", "Tak", "Nie");
		if (result)
		{
			int ID = book.Id;
			var BookToDelete = _context.Book.FirstOrDefault(p => p.Id == ID);
			if (BookToDelete != null)
			{
				_context.Book.Remove(BookToDelete);
				await _context.SaveChangesAsync();

				MainPage refreshMainPage = new MainPage();
				NavigationPage.SetHasBackButton(refreshMainPage, false);
				Navigation.PushAsync(refreshMainPage);
			}
		}
	}
	async void BookEditButton(object sender, EventArgs e)
	{
		Button button = (Button)sender;
		var book = button.BindingContext as Model.Books;
		bool result = await Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Czy na pewno chcesz zedytowaæ tê ksi¹¿kê?", "Tak", "Nie");
		if (result)
		{
			int ID = book.Id;
			EditBookPage resultsPage = new EditBookPage(ID, _context);
			Navigation.PushAsync(resultsPage);
		}
	}
}