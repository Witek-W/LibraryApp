using Library.Model;

namespace Library.Pages.RentBookPages;

public partial class RentBookQRConfirm : ContentPage
{
	private readonly LibraryDbContext _context;
	private int qr;
	private int idreader;
	private Books result;
	public RentBookQRConfirm(string qrresult,string clientid)
	{
		qr = Convert.ToInt32(qrresult);
		idreader = Convert.ToInt32(clientid);
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
			await DisplayAlert("Ostrze¿enie", $"Ksi¹¿ka jest wypo¿yczona do dnia: {result.Planned_return_date}", "Powrót");
			MainPageBack();
			return;
		} 
		if(result.Reservation == 1)
		{
			if(DateTime.Now < result.Reservation_End) 
			{
				if (idreader != result.ReservationReaderID)
				{
					await DisplayAlert("Ostrze¿enie", $"Ksi¹¿ka jest zarezerwowana dla innego u¿ytkownika", "Powrót");
					MainPageBack();
					return;
				}
			}
		}
		result.Availability = 0;
		result.Rental_date = DateTime.Now;
		result.Planned_return_date = DateTime.Now.AddDays(Days);
		result.ReaderID = idreader;
		result.Reservation = 0;
		result.Reservation_End = null;
		result.ReservationReaderID = null;
		_context.SaveChanges();
		await DisplayAlert("Powiadomienie", "Ksi¹¿ka zosta³a wypo¿yczona", "WyjdŸ");
		MainPageBack();
	}
	private void BookRentDeny(object sender, EventArgs e)
	{
		MainPageBack();
	}
	private void MainPageBack()
	{
		MainPage BrandNew = new MainPage();
		NavigationPage.SetHasBackButton(BrandNew, false);
		Navigation.PushAsync(BrandNew);
	}
}