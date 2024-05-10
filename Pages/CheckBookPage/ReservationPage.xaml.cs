using Library.Model;

namespace Library.Pages.CheckBookPage;

public partial class ReservationPage : ContentPage
{
	private LibraryDbContext _context;
	private int IDbook = 0;
	private int ReaderID;
	private Books book;
	public ReservationPage(int ID, LibraryDbContext context)
	{
		_context = context;
		InitializeComponent();
		ButtonReservation.IsEnabled = false;
		IDbook = ID;
		book = _context.Book.FirstOrDefault(p => p.Id == IDbook);
		BookName.Text = $"{book.Name}";
		Author.Text = $"{book.Author}";
		Type.Text = $"{book.Type}";

		ReturnDate.Text = $"{book.Planned_return_date.Value.Date.ToString("dd-MM-yyyy")}";
	}
	private void IDreaderEntry(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(ReaderEntry.Text))
		{
			ButtonReservation.IsEnabled = true;
		} else
		{
			ButtonReservation.IsEnabled = false;
		}
	}
	async private void ReservationSend(object sender, EventArgs e)
	{
		ReaderID = Convert.ToInt32(ReaderEntry.Text);
		if(book.Reservation == 1)
		{
			await Application.Current.MainPage.DisplayAlert("Ostrze¿enie", "Nie mo¿na zarezerwowaæ ksi¹¿ki", "WyjdŸ");
		} else
		{
			book.Reservation = 1;
			book.ReservationReaderID = ReaderID;
			_context.SaveChanges();
			await Application.Current.MainPage.DisplayAlert("Potwierdzenie", "Ksi¹¿ka zosta³a zarezerwowana", "Ok");
		}
		
		MainPage refreshMainPage = new MainPage();
		NavigationPage.SetHasBackButton(refreshMainPage, false);
		Navigation.PushAsync(refreshMainPage);
	}
}