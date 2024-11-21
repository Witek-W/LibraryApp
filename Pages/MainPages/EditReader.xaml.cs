using Library.Model;
using Library.Pages.ManageLibraryPages;
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
	private AddReaderPage _readerpage;
	//Bool walidacja
	private bool NameValid = true;
	private bool SurnameValid = true;
	private bool PhoneNumberValid = true;
	private bool CityValid = true;
	private bool StreetValid = true;
	private bool HouseNumberValid = true;
	public EditReader(Readers user)
	{
		InitializeComponent();
		editedUser = user;
		_context = new LibraryDbContext();
		_readerpage = new AddReaderPage();
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
	//Template UI
	private void ChangeUI(Label label, Frame frame, Frame background, Image image, bool isValid, string imageSource)
	{
		if (!isValid)
		{
			label.TextColor = Colors.Red;
			frame.BorderColor = Colors.Red;
			background.BackgroundColor = Colors.Red;
			image.Source = "errorvalidation.png";
		}
		else
		{
			label.TextColor = Color.FromArgb("#6f78df");
			frame.BorderColor = Colors.Gray;
			background.BackgroundColor = Color.FromArgb("#6f78df");
			image.Source = imageSource;
		}
	}
	private void ReaderInput(object sender, EventArgs e)
	{
		//Walidacja danych
		//Pole Imiê
		if(sender == ReaderName)
		{
			if(_readerpage.CheckString(ReaderName.Text,3)) {
				ChangeUI(LabelReaderName, FrameReaderName, FrameBackgroundReaderName, ImageReaderName, false, "name.png");
				NameValid = false;
			} else
			{
				ChangeUI(LabelReaderName, FrameReaderName, FrameBackgroundReaderName, ImageReaderName, true, "name.png");
				NameValid = true;
			}
		//Pole Nazwisko
		} else if(sender == ReaderSurname)
		{
			if (_readerpage.CheckString(ReaderSurname.Text, 3))
			{
				ChangeUI(LabelReaderSurname, FrameReaderSurname, FrameBackgroundReaderSurname, ImageReaderSurname, false, "name.png");
				SurnameValid = false;
			}
			else
			{
				ChangeUI(LabelReaderSurname, FrameReaderSurname, FrameBackgroundReaderSurname, ImageReaderSurname, true, "name.png");
				SurnameValid = true;
			}
		//Pole numer telefonu
		} else if(sender == ReaderPhoneNumber)
		{
			if (_readerpage.CheckString(ReaderPhoneNumber.Text, 4) || ReaderPhoneNumber.Text.Length > 9 || ReaderPhoneNumber.Text.Length < 9)
			{
				ChangeUI(LabelPhoneNumber, FramePhoneNumber, FrameBackgroundPhoneNumber, ImagePhoneNumber, false, "phone.png");
				PhoneNumberValid = false;
			}
			else
			{
				ChangeUI(LabelPhoneNumber, FramePhoneNumber, FrameBackgroundPhoneNumber, ImagePhoneNumber, true, "phone.png");
				PhoneNumberValid = true;
			}
		//Pole miejscowoœæ
		} else if(sender == ReaderCity)
		{
			if (_readerpage.CheckString(ReaderCity.Text, 1))
			{
				ChangeUI(LabelCity, FrameCity, FrameBackgroundCity, ImageCity, false, "city.png");
				CityValid = false;
			}
			else
			{
				ChangeUI(LabelCity, FrameCity, FrameBackgroundCity, ImageCity, true, "city.png");
				CityValid = true;
			}
		//Pole ulica
		} else if(sender == ReaderStreet)
		{
			if (_readerpage.CheckString(ReaderStreet.Text, 1))
			{
				ChangeUI(LabelStreet, FrameStreet, FrameBackgroundStreet, ImageStreet, false, "city.png");
				StreetValid = false;
			}
			else
			{
				ChangeUI(LabelStreet, FrameStreet, FrameBackgroundStreet, ImageStreet, true, "city.png");
				StreetValid = true;
			}
		//Pole numer domu
		} else if(sender == ReaderHouseNumber)
		{
			if (_readerpage.CheckString(ReaderHouseNumber.Text, 2))
			{
				ChangeUI(LabelHouseNumber, FrameHouseNumber, FrameBackgroundHouseNumber, ImageHouseNumber, false, "city.png");
				HouseNumberValid = false;
			}
			else
			{
				ChangeUI(LabelHouseNumber, FrameHouseNumber, FrameBackgroundHouseNumber, ImageHouseNumber, true, "city.png");
				HouseNumberValid = true;
			}
		}

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
		if((!string.IsNullOrEmpty(ReaderName.Text) && NameValid) || (!string.IsNullOrEmpty(ReaderSurname.Text) && SurnameValid) || 
			(!string.IsNullOrEmpty(ReaderPhoneNumber.Text) && PhoneNumberValid) || (!string.IsNullOrEmpty(ReaderCity.Text) && CityValid) || 
			(!string.IsNullOrEmpty(ReaderStreet.Text) && StreetValid) || (!string.IsNullOrEmpty(ReaderHouseNumber.Text) && HouseNumberValid))
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