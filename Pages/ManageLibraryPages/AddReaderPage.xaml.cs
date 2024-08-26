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
	private void ReaderInput(object sender, EventArgs e)
	{
		if(!string.IsNullOrEmpty(ReaderName.Text) && !string.IsNullOrEmpty(ReaderSurname.Text) && !string.IsNullOrEmpty(ReaderPhoneNumber.Text) &&
			!string.IsNullOrEmpty(ReaderCity.Text) && !string.IsNullOrEmpty(ReaderStreet.Text) && !string.IsNullOrEmpty(ReaderHouseNumber.Text))
		{
			AddReaderButton.IsEnabled = true;
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
			MainStackLayout.Padding = new Thickness(20, 20, 20, 260);
		}
	}
}