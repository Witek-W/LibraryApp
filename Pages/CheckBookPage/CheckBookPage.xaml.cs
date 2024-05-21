using epj.Expander.Maui;
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
	public double ScreenWidthPercentage => DeviceDisplay.MainDisplayInfo.Width * 0.8;
	public double ScreenHeightPercentage => DeviceDisplay.MainDisplayInfo.Height * 0.7;


	private void SearchResultClicked(object sender, EventArgs e)
	{
		string Author = SearchAuthor.Text;
		string Name = SearchName.Text;
		string Type = SearchType.Text;
		int? Availbility = null;
		if (!string.IsNullOrEmpty(SearchAvailbility.Text))
		{
			Availbility = Convert.ToInt32(SearchAvailbility.Text);
		}
		
		
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
		if (!string.IsNullOrEmpty(Availbility.ToString()))
		{
			query = query.Where(p => p.Availability == Availbility);
		}
		
		var results = query.ToList();
		SearchResultPage resultsPage = new SearchResultPage(results, _context);
		Navigation.PushAsync(resultsPage); 

	}
}