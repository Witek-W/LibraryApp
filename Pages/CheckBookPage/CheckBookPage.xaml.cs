using Library.Model;
using Library.ViewModel;
using Microsoft.EntityFrameworkCore.Storage;

namespace Library;

public partial class CheckBookPage : ContentPage
{
	private readonly LibraryDbContext _context;
	public CheckBookPage()
	{
		InitializeComponent();
		_context = new LibraryDbContext();

	}
	


private void SearchResultClicked(object sender, EventArgs e)
	{
		string Author = SearchAuthor.Text;
		string Name = SearchName.Text;
		string Type = SearchType.Text;
		var query = _context.Book.AsQueryable();
		if(!string.IsNullOrEmpty(Author))
		{
			query = query.Where(p => p.Author == Author);
		}
		if (!string.IsNullOrEmpty(Name))
		{
			query = query.Where(p => p.Name == Name);
		}
		if (!string.IsNullOrEmpty(Type))
		{
			query = query.Where(p => p.Type == Type);
		}
		
		var results = query.ToList();
		SearchResultPage resultsPage = new SearchResultPage(results);
		Navigation.PushAsync(resultsPage); 

	}
}