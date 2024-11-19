using Library.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls;

namespace Library.Pages.MainPages;

public partial class EditReader : ContentPage
{
	private string NewName, NewSurname, NewPhoneNumber, NewCity, NewStreet, NewHouseNumber;
	private LibraryDbContext _context;
	private int IDReader;
	private Helpers _help;
	private Readers editedUser;
	public EditReader(Readers user)
	{
		InitializeComponent();
		editedUser = user;
		_context = new LibraryDbContext();
		_help = new Helpers(Navigation);
		SettingPlaceholders();
	}
	private void SettingPlaceholders()
	{
		ReaderName.Placeholder = $"Poprzednie: {editedUser.Name}";
		ReaderSurname.Placeholder = $"Poprzednie: {editedUser.Surname}";
		ReaderPhoneNumber.Placeholder = $"Poprzednie: {editedUser.Phone_Number}";
		ReaderCity.Placeholder = $"Poprzednie: {editedUser.City}";
		ReaderStreet.Placeholder = $"Poprzednie: {editedUser.Street}";
		ReaderHouseNumber.Placeholder = $"Poprzednie: {editedUser.House_Number}";
	}
	private void ReaderInput(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(ReaderName.Text) && !string.IsNullOrEmpty(ReaderSurname.Text) && !string.IsNullOrEmpty(ReaderPhoneNumber.Text) &&
			!string.IsNullOrEmpty(ReaderCity.Text) && !string.IsNullOrEmpty(ReaderStreet.Text) && !string.IsNullOrEmpty(ReaderHouseNumber.Text))
		{
			MainStackLayout.Padding = new Thickness(20, 20, 20, 20);
			ReaderHouseNumber.IsEnabled = false;
			ReaderHouseNumber.IsEnabled = true;
			ReaderSurname.IsEnabled = false;
			ReaderSurname.IsEnabled = true;
			ReaderPhoneNumber.IsEnabled = false;
			ReaderPhoneNumber.IsEnabled = true;
			ReaderCity.IsEnabled = false;
			ReaderCity.IsEnabled = true;
			ReaderStreet.IsEnabled = false;
			ReaderStreet.IsEnabled = true;
			ReaderName.IsEnabled = false;
			ReaderName.IsEnabled = true;
		}
		if(!string.IsNullOrEmpty(ReaderName.Text) || !string.IsNullOrEmpty(ReaderSurname.Text) || !string.IsNullOrEmpty(ReaderPhoneNumber.Text) ||
			!string.IsNullOrEmpty(ReaderCity.Text) || !string.IsNullOrEmpty(ReaderStreet.Text) || !string.IsNullOrEmpty(ReaderHouseNumber.Text))
		{
			AddReaderButton.IsEnabled = true;
		} else
		{
			AddReaderButton.IsEnabled = false;
		}
	}
	private void OnEntryFocused(object sender, FocusEventArgs e)
	{
		if (e.IsFocused)
		{
			MainStackLayout.Padding = new Thickness(20, 20, 20, 260);
		}
	}
	private async void EditReaderButton(object sender, EventArgs e)
	{
		NewName = ReaderName.Text;
		NewSurname = ReaderSurname.Text;
		NewPhoneNumber = ReaderPhoneNumber.Text;
		NewCity = ReaderCity.Text;
		NewStreet = ReaderStreet.Text;
		NewHouseNumber = ReaderHouseNumber.Text;
		try
		{
			var EditReader = await _context.Readers.FirstOrDefaultAsync(p => p.Id == editedUser.Id);
			if(EditReader != null)
			{
				if(NewName != null)
				{
					EditReader.Name = NewName;
				}
				if(NewSurname != null)
				{
					EditReader.Surname = NewSurname;
				}
				if(NewPhoneNumber != null)
				{
					EditReader.Phone_Number = NewPhoneNumber;
				}
				if(NewCity != null)
				{
					EditReader.City = NewCity;
				}
				if(NewStreet != null)
				{
					EditReader.Street = NewStreet;
				}
				if(NewHouseNumber != null)
				{
					EditReader.House_Number = NewHouseNumber;
				}
				await _context.SaveChangesAsync();
				Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Dane czytelnika zosta³y zmienione", "Ok");
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