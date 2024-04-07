using Library.Model;

namespace Library.Pages.RentBookPages;

public partial class RentBookQRConfirm : ContentPage
{
	private readonly LibraryDbContext _context;
	private int qr;
	private Books result;
	public RentBookQRConfirm(string qrresult)
	{
		qr = Convert.ToInt32(qrresult);
		InitializeComponent();
		_context = new LibraryDbContext();
		result = _context.Book.FirstOrDefault(p => p.Id == Convert.ToInt32(qrresult));
		NameType.Text = $"{result.Name}";
		AuthorType.Text = $"{result.Author}";
		Type.Text = $"{result.Type}";

		ButtonConfirm.IsEnabled = false;
	}
	private void RentLengthText(object sender, EventArgs e)
	{
		if(!string.IsNullOrEmpty(RentLength.Text))
		{
			ButtonConfirm.IsEnabled = true;
		}
	}
	private async void BookRentConfirm(object sender, EventArgs e)
	{
		int Days = 0;
		if(!string.IsNullOrEmpty(RentLength.Text))
		{
			Days = Convert.ToInt32(RentLength.Text);
		}
		

		if (result.Availability == 0)
		{
			await DisplayAlert("Ostrzeøenie", $"Ksiπøka jest zarezerwowana do dnia: {result.Planned_return_date}", "Wyjdü");
		} else
		{
			result.Availability = 0;
			result.Rental_date = DateTime.Now;
			result.Planned_return_date = DateTime.Now.AddDays(Days);
			_context.SaveChanges();
			await DisplayAlert("Powiadomienie", "Ksiπøka zosta≥a wypoøyczona", "Wyjdü");
		}
		MainPage BrandNew = new MainPage();
		Navigation.PushAsync(BrandNew);
	}
	private void BookRentDeny(object sender, EventArgs e)
	{
		MainPage BrandNew = new MainPage();
		Navigation.PushAsync(BrandNew);
	}

}