using Library.Model;

namespace Library.Pages.ManageLibraryPages;

public partial class AddReaderPage : ContentPage
{
	private readonly LibraryDbContext _context;
	public AddReaderPage()
	{
		InitializeComponent();
		_context = new LibraryDbContext();
		AddReaderButton.IsEnabled = false;
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
		int ID = 0;
		using (var dbContext = new LibraryDbContext())
		{
			var newRecord = new Readers {Name = name, Surname = surname, Phone_Number = phone, City = city, Street = street, House_Number = housenumber};
			dbContext.Readers.Add(newRecord);
			dbContext.SaveChanges();
			ID = newRecord.Id;
		}
		await DisplayAlert("Powiadomienie", $"Czytelnik zosta³ dodany poprawnie. Przyznane ID: {ID}", "Powrót do strony g³ównej");
		MainPage refreshMainPage = new MainPage();
		NavigationPage.SetHasBackButton(refreshMainPage, false);
		Navigation.PushAsync(refreshMainPage);
	}
}