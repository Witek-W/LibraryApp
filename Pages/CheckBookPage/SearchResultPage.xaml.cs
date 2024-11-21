using epj.Expander.Maui;
using Library.Model;
using Library.Pages.CheckBookPage;
using Library.Pages.Popups;
using System.Globalization;

namespace Library;

public partial class SearchResultPage : ContentPage
{
	private int currentPage = 0;
	private const int pageSize = 9;
	private List<Model.Books> allbooks;
	private LibraryDbContext _context;
	private Helpers _help;
	private int allbooksCount;
	public SearchResultPage(List<Model.Books> results, LibraryDbContext context)
	{
		InitializeComponent();
		allbooks = results;
		allbooksCount = allbooks.Count();
		LoadPage(currentPage);
		PageNumberLabel.Text = (currentPage + 1).ToString();
		_context = context;
		_help = new Helpers(Navigation);
	}
	private void LoadPage(int pageNumber)
	{
		var booksForPage = allbooks.Skip(pageNumber * pageSize).Take(pageSize).ToList();
		int booksCount = booksForPage.Count();
		if(booksCount == 0)
		{
			NoBooks.IsVisible = true;
			Results.IsVisible = false;
			PrevButton.IsEnabled = false;
			NextButton.IsEnabled = false;
		} else
		{
			ResultsListView.ItemsSource = booksForPage;
			currentPage = pageNumber;

			if (currentPage == 0)
			{
				PrevButton.IsEnabled = false;
			}
			else
			{
				PrevButton.IsEnabled = true;
			}
			if (allbooksCount < (currentPage + 1) * pageSize)
			{
				NextButton.IsEnabled = false;
			}
			else
			{
				NextButton.IsEnabled = true;
			}
		}
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
		try
		{
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
		} catch
		{
			_help.ShowInternetError();
		}
		
	}
	async void BookEditButton(object sender, EventArgs e)
	{
		Button button = (Button)sender;
		var book = button.BindingContext as Model.Books;
		try
		{
			bool result = await Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Czy na pewno chcesz edytowaæ tê ksi¹¿kê?", "Tak", "Nie");
			if (result)
			{
				int ID = book.Id;
				EditBookPage resultsPage = new EditBookPage(ID, _context);
				Navigation.PushAsync(resultsPage);
			}
		}
		catch
		{
			_help.ShowInternetError();
		}
	}
	async void ReservationButton(object sender, EventArgs e)
	{
		Button button = (Button)sender;
		var book = button.BindingContext as Model.Books;
		try
		{
			bool result = await Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Czy na pewno chcesz zarezerwowaæ te ksi¹¿kê?", "Tak", "Nie");
			if (result)
			{
				int ID = book.Id;
				ReservationPage resultPage = new ReservationPage(ID, _context);
				Navigation.PushAsync(resultPage);
			}
		}
		catch
		{
			_help.ShowInternetError();
		}
	}
}