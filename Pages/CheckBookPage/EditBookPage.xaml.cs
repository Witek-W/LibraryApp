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
	//Boole walidacja
	private bool AuthorValid = true;
	private bool TypeValid = true;
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

		BookName.Text = "Tytu³ ksi¹¿ki: " + $"{result.Name}";
		Author.Text = "Autor ksi¹¿ki: " + $"{result.Author}"; 
		Type.Text = "Gatunek ksi¹¿ki: " + $"{result.Type}";
	}
	private void EditEntry(object sender, EventArgs e)
	{
		//Walidacja
		//Pole autor
		if(sender == EntryAuthor)
		{
			if (_help.CheckString(EntryAuthor.Text, 1))
			{
				_help.ChangeUI(LabelAuthorBook, FrameAuthorBook, FrameBackgroundAuthorBook, ImageAuthorBook, false, "user.png");
				AuthorValid = false;
			}
			else
			{
				_help.ChangeUI(LabelAuthorBook, FrameAuthorBook, FrameBackgroundAuthorBook, ImageAuthorBook, true, "user.png");
				AuthorValid = true;
			}
		//Pole gatunek
		}else if(sender == EntryType)
		{
			if (_help.CheckString(EntryType.Text, 3))
			{
				_help.ChangeUI(LabelTypeBook, FrameTypeBook, FrameBackgroundTypeBook, ImageTypeBook, false, "booktype.png");
				TypeValid = false;
			}
			else
			{
				_help.ChangeUI(LabelTypeBook, FrameTypeBook, FrameBackgroundTypeBook, ImageTypeBook, true, "booktype.png");
				TypeValid = true;
			}
		}

		if(!string.IsNullOrEmpty(EntryName.Text) || (!string.IsNullOrEmpty(EntryAuthor.Text) && AuthorValid) || (!string.IsNullOrEmpty(EntryType.Text) && TypeValid))
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
				MainStackLayout.Padding = new Thickness(20, 20, 20, 20);
				EntryName.IsEnabled = false;
				EntryName.IsEnabled = true;
				EntryAuthor.IsEnabled = false;
				EntryAuthor.IsEnabled = true;
				EntryType.IsEnabled = false;
				EntryType.IsEnabled = true;

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
	private void OnFocused(object sender, FocusEventArgs e)
	{
		if (e.IsFocused)
		{
			MainStackLayout.Padding = new Thickness(20, 20, 20, 260);
		}
	}
}