using CommunityToolkit.Mvvm.ComponentModel;
using epj.Expander.Maui;
using Library.Model;
using Library.Pages.CheckBookPage;
using Library.Pages.Popups;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;

namespace Library;

public partial class SearchResultPage : ContentPage
{
	private const int pageSize = 13;
	private bool isLoading = false;
	public List<Item> _allBooksList;
	public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
	private LibraryDbContext _context;
	private Helpers _help;
	public class Item
	{
		public string Name { get; set; }
		public string Author { get; set; }
		public string Type { get; set; }
		public int Availability { get; set; }
	}
	public SearchResultPage(List<Model.Books> results, LibraryDbContext context)
	{
		BindingContext = this;
		InitializeComponent();
		_allBooksList = results.Select(p => new Item
		{
			Name = p.Name,
			Author = p.Author,
			Type = p.Type,
			Availability = p.Availability,
		}).ToList();
		LoadMoreItems();
		_context = context;
		_help = new Helpers(Navigation);
	}
	
	private async void LoadMoreItems()
	{
		Items.Clear();
		var records = _allBooksList.Take(pageSize).ToList();
		foreach(var record in records)
		{
			Items.Add(record);
		}
	}
	public async void LoadMoreRecords(object sender, EventArgs e)
	{
		if (isLoading) return;
		if (Items.Count >= _allBooksList.Count)
		{
			LoadingIcon.IsVisible = false;
			collection.RemainingItemsThreshold = -1;
			return;
		}
		if(_allBooksList?.Count > 0)
		{
			isLoading = true;
			await Task.Delay(1000);
			var records = _allBooksList.Skip(Items.Count).Take(pageSize).ToList();
			foreach (var record in records)
			{
				Items.Add(record);
			}
			isLoading = false;
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