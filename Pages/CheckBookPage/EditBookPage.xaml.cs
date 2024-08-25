using Library.Model;

namespace Library.Pages.CheckBookPage;

public partial class EditBookPage : ContentPage
{
	private LibraryDbContext _context;
	private string NameEntry;
	private string AuthorEntry;
	private string TypeEntry;
	private int IDbook = 0;
	private Helpers _help;
	public EditBookPage(int ID, LibraryDbContext context)
	{
		_context = context;
		InitializeComponent();
		_help = new Helpers(Navigation);
		EditButtonConfirm.IsEnabled = false;
		IDbook = ID;
		Books result = null;
		try
		{
			result = _context.Book.FirstOrDefault(p => p.Id == IDbook);
		} catch
		{
			_help.ShowInternetError();
		}

		BookName.Text = $"{result.Name}";
		Author.Text = $"{result.Author}";
		Type.Text = $"{result.Type}";
	}
	private void EditEntry(object sender, EventArgs e)
	{
		if(!string.IsNullOrEmpty(EntryName.Text) || !string.IsNullOrEmpty(EntryAuthor.Text) || !string.IsNullOrEmpty(EntryType.Text))
		{
			EditButtonConfirm.IsEnabled = true;
		} else
		{
			EditButtonConfirm.IsEnabled = false;
		}
	}
	void EditBook(object sender, EventArgs e)
	{
		NameEntry = EntryName.Text;
		AuthorEntry = EntryAuthor.Text;
		TypeEntry = EntryType.Text;
		try
		{
			var bookToUpdate = _context.Book.FirstOrDefault(p => p.Id == IDbook);
			if (bookToUpdate != null)
			{
				if (NameEntry != null)
				{
					bookToUpdate.Name = NameEntry;
				}
				if (AuthorEntry != null)
				{
					bookToUpdate.Author = AuthorEntry;
				}
				if (TypeEntry != null)
				{
					bookToUpdate.Type = TypeEntry;
				}
				_context.SaveChanges();
				Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Ksi¹¿ka zosta³a zedytowana", "Ok");
				MainPage refreshMainPage = new MainPage();
				NavigationPage.SetHasBackButton(refreshMainPage, false);
				Navigation.PushAsync(refreshMainPage);
			}
		}
		catch
		{
			_help.ShowInternetError();
		}
	}
}