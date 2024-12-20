using Library.Model;
using Plugin.NFC;
using Library.Pages.Popups;
using Microsoft.Maui.Platform;
using System.Security.Cryptography;
using System.Diagnostics;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Library;

namespace Library.Pages.ManageLibraryPages;

public partial class AddReaderPage : ContentPage
{
	private readonly LibraryDbContext _context;
	private int ID;
	private NFCNdefTypeFormat _type;
	private WriteNFCWaiting _writenfcwaiting;
	private Helpers _help;
	//Bools entry validation
	private bool ReaderNameValid = true;
	private bool ReaderSurnameValid = true;
	private bool ReaderPhoneNumberValid = true;
	private bool ReaderCityValid = true;
	private bool ReaderStreetValid = true;
	private bool ReaderHouseNumberValid = true;
	public AddReaderPage()
	{
		CrossNFC.Current.OnTagDiscovered += Current_OnTagDiscovered;
		CrossNFC.Current.OnMessagePublished += Current_OnMessagePublished;
		InitializeComponent();
		_help = new Helpers(Navigation);
		_context = new LibraryDbContext();
		CheckNFC();
    }
	private async Task CheckNFC()
	{
        if (!CrossNFC.Current.IsAvailable)
        {
            await DisplayAlert("Ostrze�enie", "NFC jest niedost�pne. Nie jeste� w stanie doda� nowego czytelnika", "Powr�t");
            MainPage BrandNew = new MainPage();
            NavigationPage.SetHasBackButton(BrandNew, false);
            await Navigation.PushAsync(BrandNew);
        }
        else if (!CrossNFC.Current.IsEnabled)
        {
			while (!CrossNFC.Current.IsEnabled)
			{
				await DisplayAlert("Ostrze�enie", "NFC jest wy��czone. W��cz NFC i kliknij Kontynuuj", "Kontynuuj");
			}
        }
    }
	async Task ShowPopup()
	{
		_writenfcwaiting = new WriteNFCWaiting();
		await Navigation.PushModalAsync(_writenfcwaiting);
	}
	async Task ClosePopup()
	{
		if (_writenfcwaiting != null)
		{
			await _writenfcwaiting.Change();
			_writenfcwaiting = null;
		}
	}
	private void ReaderInput(object sender, EventArgs e)
	{
		//Sprawdzanie i zmiana pola: Imi�
		if(sender == ReaderName)
		{
			if (_help.CheckString(ReaderName.Text, 3))
			{
				_help.ChangeUI(LabelReaderName, FrameReaderName, FrameBackgroundReaderName, ImageReaderName, false, "name.png");
				ReaderNameValid = false;
			}
			else
			{
				_help.ChangeUI(LabelReaderName, FrameReaderName, FrameBackgroundReaderName, ImageReaderName, true, "name.png");
				ReaderNameValid = true;
			}
		//Sprawdzanie i zmiana pola: Nazwisko
		} else if(sender == ReaderSurname)
		{
			if (_help.CheckString(ReaderSurname.Text, 3))
			{
				_help.ChangeUI(LabelReaderSurname, FrameReaderSurname, FrameBackgroundReaderSurname, ImageReaderSurname, false, "name.png");
				ReaderSurnameValid = false;
			}
			else
			{
				_help.ChangeUI(LabelReaderSurname, FrameReaderSurname, FrameBackgroundReaderSurname, ImageReaderSurname, true, "name.png");
				ReaderSurnameValid = true;
			}
		//Sprawdzanie i zmiana pola: Numer telefonu
		} else if(sender == ReaderPhoneNumber)
		{
			if (_help.CheckString(ReaderPhoneNumber.Text, 4) || ReaderPhoneNumber.Text.Length > 9 || ReaderPhoneNumber.Text.Length < 9)
			{
				_help.ChangeUI(LabelPhoneNumber, FramePhoneNumber, FrameBackgroundPhoneNumber, ImagePhoneNumber, false, "phone.png");
				ReaderPhoneNumberValid = false;
			}
			else
			{
				_help.ChangeUI(LabelPhoneNumber, FramePhoneNumber, FrameBackgroundPhoneNumber, ImagePhoneNumber, true, "phone.png");
				ReaderPhoneNumberValid = true;
			}
		//Sprawdzanie i zmiana pola: Miejscowo��
		} else if(sender == ReaderCity)
		{
			if (_help.CheckString(ReaderCity.Text, 1))
			{
				_help.ChangeUI(LabelCity, FrameCity, FrameBackgroundCity, ImageCity, false, "city.png");
				ReaderCityValid = false;
			}
			else
			{
				_help.ChangeUI(LabelCity, FrameCity, FrameBackgroundCity, ImageCity, true, "city.png");
				ReaderCityValid = true;
			}
		//Sprawdzanie i zmiana pola: Ulica
		} else if(sender == ReaderStreet)
		{
			if (_help.CheckString(ReaderStreet.Text, 1))
			{
				_help.ChangeUI(LabelStreet, FrameStreet, FrameBackgroundStreet, ImageStreet, false, "city.png");
				ReaderStreetValid = false;
			}
			else
			{
				_help.ChangeUI(LabelStreet, FrameStreet, FrameBackgroundStreet, ImageStreet, true, "city.png");
				ReaderStreetValid = true;
			}
		//Sprawdzanie i zmiana pola: Numer domu
		} else if(sender == ReaderHouseNumber)
		{
			if (_help.CheckString(ReaderHouseNumber.Text, 2))
			{
				_help.ChangeUI(LabelHouseNumber, FrameHouseNumber, FrameBackgroundHouseNumber, ImageHouseNumber, false, "city.png");
				ReaderHouseNumberValid = false;
			}
			else
			{
				_help.ChangeUI(LabelHouseNumber, FrameHouseNumber, FrameBackgroundHouseNumber, ImageHouseNumber, true, "city.png");
				ReaderHouseNumberValid = true;
			}
		}
		//Sprawdzanie czy wszystkie entry s� uzupe�nione
		if (!string.IsNullOrEmpty(ReaderName.Text) && !string.IsNullOrEmpty(ReaderSurname.Text) && !string.IsNullOrEmpty(ReaderPhoneNumber.Text) &&
			!string.IsNullOrEmpty(ReaderCity.Text) && !string.IsNullOrEmpty(ReaderStreet.Text) && !string.IsNullOrEmpty(ReaderHouseNumber.Text)
			&& ReaderNameValid && ReaderSurnameValid && ReaderPhoneNumberValid && ReaderCityValid && ReaderStreetValid && ReaderHouseNumberValid)
		{
			AddReaderButton.IsEnabled = true;
		} else
		{
			AddReaderButton.IsEnabled = false;
		}
	}
	private async void AddNewReader(object sender, EventArgs e)
	{
		string name = ReaderName.Text;
		string surname = ReaderSurname.Text;
		string phone = ReaderPhoneNumber.Text;
		string city = ReaderCity.Text;
		string street = ReaderStreet.Text;
		string housenumber = ReaderHouseNumber.Text;
        try
		{
            var checkUser = _context.Readers.FirstOrDefault(p => p.Phone_Number == phone);
            if (checkUser != null)
            {
                await DisplayAlert("Ostrze�enie", "Ten numer telefonu jest ju� przypisany do innego czytelnika", "Powr�t");
                MainPage BrandNew = new MainPage();
                NavigationPage.SetHasBackButton(BrandNew, false);
                await Navigation.PushAsync(BrandNew);
                return;
            } else
			{
				using (var dbContext = new LibraryDbContext())
				{
					var newRecord = new Readers { Name = name, Surname = surname, Phone_Number = phone, City = city, Street = street, House_Number = housenumber };
					dbContext.Readers.Add(newRecord);
					dbContext.SaveChanges();
					ID = newRecord.Id;
				}
			}

		}
		catch
		{
			_help.ShowInternetError();
			return;
		}
        //await DisplayAlert("Powiadomienie", $"Przy�� kart� NFC aby zapisa� dane czytelnika {ID}", "Dalej");
        //WriteNfcTag();
        MainStackLayout.Padding = new Thickness(20, 20, 20, 20);
        //Spos�b na znikni�cie klawiatury
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
        ReaderHouseNumber.IsEnabled = false;
		await ShowPopup();
		await Publish(NFCNdefTypeFormat.WellKnown);

	}
	//Write ID to NFC Tag
	async Task Publish(NFCNdefTypeFormat? type = null)
	{
		CrossNFC.Current.StartListening();
		_type = NFCNdefTypeFormat.Empty;
		if (type.HasValue) _type = type.Value;
		CrossNFC.Current.StartPublishing(!type.HasValue);
	}
	private async void Current_OnTagDiscovered(ITagInfo tagInfo, bool format)
	{
		if (!CrossNFC.Current.IsWritingTagSupported) return;
		try
		{
			if (ID != 0)
			{
				NFCNdefRecord record = new NFCNdefRecord
				{
					TypeFormat = NFCNdefTypeFormat.WellKnown,
					MimeType = "application/com.companyname.Library",
					Payload = NFCUtils.EncodeToByteArray(ID.ToString()),
					LanguageCode = "en"
				};
				tagInfo.Records = new[] { record };

				CrossNFC.Current.PublishMessage(tagInfo);
			}
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
		}
	}
	async void Current_OnMessagePublished(ITagInfo tagInfo)
	{
		CrossNFC.Current.StopPublishing();
		await ClosePopup();
		ReaderName.Text = string.Empty;
		ReaderSurname.Text = string.Empty;
		ReaderPhoneNumber.Text = string.Empty;
		ReaderCity.Text = string.Empty;
		ReaderStreet.Text = string.Empty;
		ReaderHouseNumber.Text = string.Empty;
		ID = 0;
		await Navigation.PopAsync();
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();

		if (CrossNFC.IsSupported)
		{
			CrossNFC.Current.StartListening();
		}
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();

		if (CrossNFC.IsSupported)
		{
			CrossNFC.Current.StopListening();
		}
	}

	//Aktualizowanie strony przy w��czonej klawiaturze
	private void OnEntryFocused(object sender, FocusEventArgs e)
	{
		if (e.IsFocused)
		{
            double screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
            MainStackLayout.Padding = new Thickness(20, 20, 20, screenHeight * 0.32);
		}
	}
}