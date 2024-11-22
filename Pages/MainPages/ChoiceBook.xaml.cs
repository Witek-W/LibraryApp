using Library.Model;
using Library.Pages.CheckBookPage;
using Microsoft.Maui.Controls;

namespace Library.Pages.MainPages;

public partial class ChoiceBook : ContentPage
{
	private LibraryDbContext _context;
	private Helpers _help;
	private int IDBook;
	public ChoiceBook(int ID)
	{
		_context = new LibraryDbContext();
		_help = new Helpers(Navigation);
		IDBook = ID;
		InitializeComponent();
		Books result = null;
		try
		{
			result = _context.Book.FirstOrDefault(p => p.Id == IDBook);
			if(result == null)
			{
				_help.ShowSQLError();
				return;
			}
		}
		catch
		{
			_help.ShowInternetError();
		}
		BookName.Text = "Tytu³ ksi¹¿ki: " + $"{result.Name}";
		Author.Text = "Autor ksi¹¿ki: " + $"{result.Author}";
		Type.Text = "Gatunek ksi¹¿ki: " + $"{result.Type}";
	}
	private async void DeleteBook(object sender, EventArgs e)
	{
		await DeleteBook(IDBook);
		MainPage refreshMainPage = new MainPage();
		NavigationPage.SetHasBackButton(refreshMainPage, false);
		Navigation.PushAsync(refreshMainPage);
	}
	private void EditBook(object sender, EventArgs e)
	{
		EditBookPage resultsPage = new EditBookPage(IDBook, _context);
		Navigation.PushAsync(resultsPage);
	}
	private async Task DeleteBook(int ID)
	{
		var BookToDelete = _context.Book.FirstOrDefault(p => p.Id == ID);
		if(BookToDelete != null)
		{
			_context.Book.Remove(BookToDelete);
			_context.SaveChangesAsync();
		}
	}
}