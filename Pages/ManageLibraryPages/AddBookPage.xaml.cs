using Library.Model;
using Microsoft.EntityFrameworkCore;
using QRCoder;

namespace Library;

public partial class AddBookPage : ContentPage
{
	
	public AddBookPage()
	{
		InitializeComponent();
		AddBookButton.IsEnabled = false;
	}

	private void InputBook(object sender, EventArgs e)
	{
		if(!string.IsNullOrEmpty(NameBook.Text) && !string.IsNullOrEmpty(AuthorBook.Text) && !string.IsNullOrEmpty(TypeBook.Text))
		{
			AddBookButton.IsEnabled = true;
		} else
		{
			AddBookButton.IsEnabled = false;
		}
	}

	private async void AddBookClicked(object sender, EventArgs e)
	{
		string name = NameBook.Text;
		string author = AuthorBook.Text;
		string type = TypeBook.Text;
		string IDrecord;
		using (var dbContext = new LibraryDbContext())
		{
			var newRecord = new Books { Name = name, Author = author, Type = type, Availability = 1, Rental_date = null, Planned_return_date = null, ReaderID = null};
			dbContext.Book.Add(newRecord);
			dbContext.SaveChanges();
			IDrecord = newRecord.Id.ToString();
		}

		await DisplayAlert("Powiadomienie", "Ksi¹¿ka dodana pomyœlnie", "Pobierz QR");
		QRCodeGenerator qrGenerator = new QRCodeGenerator();
		QRCodeData qrCodeData = qrGenerator.CreateQrCode(IDrecord, QRCodeGenerator.ECCLevel.H);
		PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
		byte[] qrCodeBytes = qRCode.GetGraphic(10);
		
		string fileName = $"kodQR_{name}_{author}_{type}.png";
		if(DeviceInfo.Platform == DevicePlatform.Android)
		{
			

		} else
		{
			string filePath = Path.Combine("C:\\Users\\Witek\\Desktop\\6 semestr\\Inzynierka\\LibraryNowe\\Zdjecia", fileName);
			File.WriteAllBytes(filePath, qrCodeBytes);
		}

		
		

		NameBook.Text = string.Empty;
		AuthorBook.Text = string.Empty;
		TypeBook.Text = string.Empty;
	}
}