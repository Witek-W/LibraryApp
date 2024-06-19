using Library.Model;
using Plugin.NFC;

namespace Library.Pages.ManageLibraryPages;

public partial class AddReaderPage : ContentPage
{
	private readonly LibraryDbContext _context;
	private readonly NFC _nfc;
	private int ID;
	public AddReaderPage()
	{
		InitializeComponent();
		_context = new LibraryDbContext();
		AddReaderButton.IsEnabled = false;
		_nfc = new NFC();
		_nfc.MessagePublished += NFC_MessagePublished;

	}
	protected override void OnAppearing()
	{
		base.OnAppearing();

		if (CrossNFC.IsSupported)
		{
			_nfc.ReadNfcTag();
		}
	}
	
	protected override void OnDisappearing()
	{
		base.OnDisappearing();

		if (CrossNFC.IsSupported)
		{
			_nfc.StopReadNfcTag();
		}
	}
	private void NFC_MessagePublished(object sender, int id)
	{
		_nfc.StopWriteNfcTag();
		MainPage refreshMainPage = new MainPage();
		NavigationPage.SetHasBackButton(refreshMainPage, false);
		Navigation.PushAsync(refreshMainPage);
	}
	
	private void ReaderInput(object sender, EventArgs e)
	{
		if(!string.IsNullOrEmpty(ReaderName.Text) && !string.IsNullOrEmpty(ReaderSurname.Text) && !string.IsNullOrEmpty(ReaderPhoneNumber.Text) &&
			!string.IsNullOrEmpty(ReaderCity.Text) && !string.IsNullOrEmpty(ReaderStreet.Text) && !string.IsNullOrEmpty(ReaderHouseNumber.Text))
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
		using (var dbContext = new LibraryDbContext())
		{
			var newRecord = new Readers {Name = name, Surname = surname, Phone_Number = phone, City = city, Street = street, House_Number = housenumber};
			dbContext.Readers.Add(newRecord);
			dbContext.SaveChanges();
			ID = newRecord.Id;
		}
		await DisplayAlert("Powiadomienie", $"Przy³ó¿ kartê NFC aby zapisaæ dane czytelnika {ID}", "Dalej");
		_nfc.WriteNfcTag(ID);
	}
	
}