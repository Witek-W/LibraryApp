using Library.Model;

namespace Library.Pages.ReturnBookPages;

public partial class ReturnBookQRConfirm : ContentPage
{
	private readonly LibraryDbContext _context;
	private int qr;
	private Books result;
	private int ReservationBookLength = 2;
	public ReturnBookQRConfirm(string qrresult)
	{
		InitializeComponent();
		_context = new LibraryDbContext();
		qr = Convert.ToInt32(qrresult);
		result = _context.Book.FirstOrDefault(p => p.Id == qr);
		ManualResultName.Text = $"{result.Name}";
		ManualResultAuthor.Text = $"{result.Author}";
		ManualResultType.Text = $"{result.Type}";
	}
	private void ReturnToMainPage(object sender, EventArgs e)
	{
		MainPage BrandNew = new MainPage();
		NavigationPage.SetHasBackButton(BrandNew, false);
		Navigation.PushAsync(BrandNew);
	}
	private async void ReturnBookClicked(object sender, EventArgs e)
	{
		if(result.Availability == 0)
		{
			result.Availability = 1;
			result.Rental_date = null;
			result.Planned_return_date = null;
			result.ReaderID = null;
			if(result.Reservation == 1)
			{
				DateTime dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(ReservationBookLength);
				result.Reservation_End = dateTime;
			}
			_context.SaveChanges();
			await DisplayAlert("Powiadomienie", "Ksi��ka zosta�a zwr�cona", "Wyjd�");
		} else
		{
			await DisplayAlert("Ostrze�enie", "Ksi��ka nie jest zarezerwowana", "Wyjd�");
		}
		MainPage BrandNew = new MainPage();
		NavigationPage.SetHasBackButton(BrandNew, false);
		Navigation.PushAsync(BrandNew);
	}
}