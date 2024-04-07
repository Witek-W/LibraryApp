using Library.Model;

namespace Library.Pages.ReturnBookPages;

public partial class ReturnBookQRConfirm : ContentPage
{
	private readonly LibraryDbContext _context;
	private int qr;
	private Books result;
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
		Navigation.PushAsync(BrandNew);
	}
	private async void ReturnBookClicked(object sender, EventArgs e)
	{
		if(result.Availability == 0)
		{
			result.Availability = 1;
			_context.SaveChanges();
			await DisplayAlert("Powiadomienie", "Ksiπøka zosta≥a zwrÛcona", "Wyjdü");
		} else
		{
			await DisplayAlert("Ostrzeøenie", "Ksiπøka nie jest zarezerwowana", "Wyjdü");
		}
		MainPage BrandNew = new MainPage();
		Navigation.PushAsync(BrandNew);
	}
}