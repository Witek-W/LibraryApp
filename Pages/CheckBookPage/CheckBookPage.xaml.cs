using epj.Expander.Maui;
using Library.Model;
using Library.Pages.MainPages;
using Library.Pages.Popups;
using Library.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Library;

public partial class CheckBookPage : ContentPage
{
	private readonly LibraryDbContext _context;
	private int? Availbility = null;
	private Helpers _help;
	private Loading _load;
	public CheckBookPage()
	{
		InitializeComponent();
		_context = new LibraryDbContext();
		_help = new Helpers(Navigation);
	}
	//public double ScreenWidthPercentage => DeviceDisplay.MainDisplayInfo.Width * 0.8;
	//public double ScreenHeightPercentage => DeviceDisplay.MainDisplayInfo.Height * 0.7;

	private void OnSwitchToggledAval(object sender, ToggledEventArgs e)
	{
		if(e.Value == true)
		{
			Availbility = 1;
		} else
		{
			Availbility = null;
		}
	}
	async Task ShowPopup()
	{
		_load = new Loading();
		await Navigation.PushModalAsync(_load);
	}
	private async void SearchResultClicked(object sender, EventArgs e)
	{
		SearchAuthor.IsEnabled = false;
		SearchAuthor.IsEnabled = true;
		SearchName.IsEnabled = false;
		SearchName.IsEnabled = true;
		SearchType.IsEnabled = false;
		SearchType.IsEnabled = true;
		await ShowPopup();
		string Author = SearchAuthor.Text;
		string Name = SearchName.Text;
		string Type = SearchType.Text;
		try
		{
			var	query = _context.Book.AsQueryable();
		
		if (!string.IsNullOrEmpty(Author))
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
			var results =  await query.ToListAsync();
			var resultsPage = new SearchResultPage(results, _context);
			await Navigation.PushAsync(resultsPage);
			await Navigation.PopModalAsync();
		}
		catch
		{
			await Navigation.PopModalAsync();
			_help.ShowInternetError();
		}
	}
}