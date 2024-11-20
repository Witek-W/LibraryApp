using Library.Model;
using Plugin.NFC;
using Library.Pages.Popups;
using Microsoft.Maui.Platform;
using System.Security.Cryptography;
using System.Diagnostics;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Library;
using System.Text.RegularExpressions;

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
		AddReaderButton.IsEnabled = false;
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
	//Regex
	bool CheckString(string input, int variant)
	{
		switch (variant)
		{
			//Sprawdzanie czy string ma liczby i znaki specjalnie z wy³¹czeniem "-" i spacji
			case 1:
				var regexNumbers = new Regex(@"[^a-zA-Z¹æê³ñóœŸ¿¥ÆÊ£ÑÓŒ¯\s\-]");
				if (regexNumbers.IsMatch(input)) return true;
				return false;
			//Sprawdzanie znaków specjalnych
			case 2:
				var regexSpecial = new Regex(@"[^\w]");
				if (regexSpecial.IsMatch(input)) return true;
				return false;
			//Sprawdzanie czy ma liczby i znaki specjalne
			case 3:
				var regexNumberSpecial = new Regex(@"[\d\W]");
				if (regexNumberSpecial.IsMatch(input)) return true;
				return false;
			//Sprawdzanie czy ma znaki i litery
			case 4:
				foreach(char c in input)
				{
					if(!char.IsDigit(c))
					{
						return true;
					}
				}
				return false;
			default:
				return true;
		}
	}
	//Template do zmiany UI przy b³êdnej walidacji
	private void ChangeUI(Label label, Frame frame, Frame background, Image image, bool isValid, string imageSource)
	{
		if(!isValid)
		{
			label.TextColor = Colors.Red;
			frame.BorderColor = Colors.Red;
			background.BackgroundColor = Colors.Red;
			image.Source = "errorvalidation.png";
		} else
		{
			label.TextColor = Color.FromArgb("#6f78df");
			frame.BorderColor = Colors.Gray;
			background.BackgroundColor = Color.FromArgb("#6f78df");
			image.Source = imageSource;
		}
	}
	private void ReaderInput(object sender, EventArgs e)
	{
		//Sprawdzanie i zmiana pola: Imiê
		if(sender == ReaderName)
		{
			if (CheckString(ReaderName.Text, 3))
			{
				ChangeUI(LabelReaderName, FrameReaderName, FrameBackgroundReaderName, ImageReaderName, false, "name.png");
				ReaderNameValid = false;
			}
			else
			{
				ChangeUI(LabelReaderName, FrameReaderName, FrameBackgroundReaderName, ImageReaderName, true, "name.png");
				ReaderNameValid = true;
			}
		//Sprawdzanie i zmiana pola: Nazwisko
		} else if(sender == ReaderSurname)
		{
			if (CheckString(ReaderSurname.Text, 3))
			{
				ChangeUI(LabelReaderSurname, FrameReaderSurname, FrameBackgroundReaderSurname, ImageReaderSurname, false, "name.png");
				ReaderSurnameValid = false;
			}
			else
			{
				ChangeUI(LabelReaderSurname, FrameReaderSurname, FrameBackgroundReaderSurname, ImageReaderSurname, true, "name.png");
				ReaderSurnameValid = true;
			}
		//Sprawdzanie i zmiana pola: Numer telefonu
		} else if(sender == ReaderPhoneNumber)
		{
			if (CheckString(ReaderPhoneNumber.Text, 4) || ReaderPhoneNumber.Text.Length > 9)
			{
				ChangeUI(LabelPhoneNumber, FramePhoneNumber, FrameBackgroundPhoneNumber, ImagePhoneNumber, false, "phone.png");
				ReaderPhoneNumberValid = false;
			}
			else
			{
				ChangeUI(LabelPhoneNumber, FramePhoneNumber, FrameBackgroundPhoneNumber, ImagePhoneNumber, true, "phone.png");
				ReaderPhoneNumberValid = true;
			}
		//Sprawdzanie i zmiana pola: Miejscowoœæ
		} else if(sender == ReaderCity)
		{
			if (CheckString(ReaderCity.Text, 1))
			{
				ChangeUI(LabelCity, FrameCity, FrameBackgroundCity, ImageCity, false, "city.png");
				ReaderCityValid = false;
			}
			else
			{
				ChangeUI(LabelCity, FrameCity, FrameBackgroundCity, ImageCity, true, "city.png");
				ReaderCityValid = true;
			}
		//Sprawdzanie i zmiana pola: Ulica
		} else if(sender == ReaderStreet)
		{
			if (CheckString(ReaderStreet.Text, 1))
			{
				ChangeUI(LabelStreet, FrameStreet, FrameBackgroundStreet, ImageStreet, false, "city.png");
				ReaderStreetValid = false;
			}
			else
			{
				ChangeUI(LabelStreet, FrameStreet, FrameBackgroundStreet, ImageStreet, true, "city.png");
				ReaderStreetValid = true;
			}
		//Sprawdzanie i zmiana pola: Numer domu
		} else if(sender == ReaderHouseNumber)
		{
			if (CheckString(ReaderHouseNumber.Text, 2))
			{
				ChangeUI(LabelHouseNumber, FrameHouseNumber, FrameBackgroundHouseNumber, ImageHouseNumber, false, "city.png");
				ReaderHouseNumberValid = false;
			}
			else
			{
				ChangeUI(LabelHouseNumber, FrameHouseNumber, FrameBackgroundHouseNumber, ImageHouseNumber, true, "city.png");
				ReaderHouseNumberValid = true;
			}
		}
		//Sprawdzanie czy wszystkie entry s¹ uzupe³nione
		if (!string.IsNullOrEmpty(ReaderName.Text) && !string.IsNullOrEmpty(ReaderSurname.Text) && !string.IsNullOrEmpty(ReaderPhoneNumber.Text) &&
			!string.IsNullOrEmpty(ReaderCity.Text) && !string.IsNullOrEmpty(ReaderStreet.Text) && !string.IsNullOrEmpty(ReaderHouseNumber.Text)
			&& ReaderNameValid && ReaderSurnameValid && ReaderPhoneNumberValid && ReaderCityValid && ReaderStreetValid && ReaderHouseNumberValid)
		{
			AddReaderButton.IsEnabled = true;
			MainStackLayout.Padding = new Thickness(20, 20, 20, 20);
			//Sposób na znikniêcie klawiatury
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
			using (var dbContext = new LibraryDbContext())
			{
				var newRecord = new Readers { Name = name, Surname = surname, Phone_Number = phone, City = city, Street = street, House_Number = housenumber };
				dbContext.Readers.Add(newRecord);
				dbContext.SaveChanges();
				ID = newRecord.Id;
			}
		}
		catch
		{
			_help.ShowInternetError();
			return;
		}
		//await DisplayAlert("Powiadomienie", $"Przy³ó¿ kartê NFC aby zapisaæ dane czytelnika {ID}", "Dalej");
		//WriteNfcTag();
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
	private void WriteNfcTag()
	{
		CrossNFC.Current.StartListening();
		CrossNFC.Current.StartPublishing();
	}
	private void StopWriteNfcTag()
	{
		CrossNFC.Current.StopListening();
		CrossNFC.Current.StopPublishing();
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

	//Aktualizowanie strony przy w³¹czonej klawiaturze
	private void OnEntryFocused(object sender, FocusEventArgs e)
	{
		if (e.IsFocused)
		{
			MainStackLayout.Padding = new Thickness(20, 20, 20, 280);
		}
	}
}