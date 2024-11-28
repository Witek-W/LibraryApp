using Library.Model;

namespace Library.Pages.RentBookPages;

public partial class RentBookQRConfirm : ContentPage
{
	private readonly LibraryDbContext _context;
	private int qr;
	private int idreader;
	private Books result;
	private Helpers _help;
	private bool LenghtValid = true;
	public RentBookQRConfirm(string qrresult,string clientid)
	{
		qr = Convert.ToInt32(qrresult);
		idreader = Convert.ToInt32(clientid);
		InitializeComponent();
		_help = new Helpers(Navigation);
		_context = new LibraryDbContext();
		try
		{
			result = _context.Book.FirstOrDefault(p => p.Id == Convert.ToInt32(qrresult));
			if(result == null)
			{
				_help.ShowSQLError();
				return;
			}
			NameType.Text = "Tytu³ ksi¹¿ki: " + $"{result.Name}";
			AuthorType.Text = "Autor ksi¹¿ki: " + $"{result.Author}";
			Type.Text = "Gatunek ksi¹¿ki: " + $"{result.Type}";
			ButtonConfirm.IsEnabled = false;
		}
		catch
		{
			_help.ShowInternetError();
		}
	}
	private void RentLengthText(object sender, EventArgs e)
	{
		//Walidacja liczby
		if(_help.CheckString(RentLength.Text,4))
		{
			_help.ChangeUI(LabelRentLength, FrameRentLength, FrameBackgroundRentLength, ImageRentLength, false, "calendar.png");
			LenghtValid = false;
		} else
		{
			_help.ChangeUI(LabelRentLength, FrameRentLength, FrameBackgroundRentLength, ImageRentLength, true, "calendar.png");
			LenghtValid = true;
		}

		if(!string.IsNullOrEmpty(RentLength.Text) && LenghtValid)
		{
			ButtonConfirm.IsEnabled = true;
		} else
		{
			ButtonConfirm.IsEnabled = false;
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
		try
		{
			result.Availability = 0;
			result.Rental_date = DateTime.Now;
			result.Planned_return_date = DateTime.Now.AddDays(Days);
			result.ReaderID = idreader;
			result.Reservation = 0;
			result.Reservation_End = null;
			result.ReservationReaderID = null;
			_context.SaveChanges();
			RentLength.IsEnabled = false;
			RentLength.IsEnabled = true;
			await DisplayAlert("Powiadomienie", "Ksi¹¿ka zosta³a wypo¿yczona", "WyjdŸ");
			MainPageBack();
		}
		catch
		{
			_help.ShowInternetError();
		}
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