using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Library.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls.PlatformConfiguration;
using QRCoder;
using System.Text;
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
		await DisplayAlert("Powiadomienie", "Ksi¹¿ka dodana pomyœlnie", "Wygeneruj kod QR");
		QRCodeGenerator qrGenerator = new QRCodeGenerator();
		QRCodeData qrCodeData = qrGenerator.CreateQrCode(IDrecord, QRCodeGenerator.ECCLevel.H);
		PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
		byte[] qrCodeBytes = qRCode.GetGraphic(10);
		
		string fileName = $"kodQR_{name}_{author}_{type}_{IDrecord}.png";
		//Problem ze znalezieniem pliku, rozwi¹zanie chwilowe
		//Tutaj jest credentialjson ale na razie ze wzglêdów bezpieczeñstwa jest usuniêty
		string folderId = "***REMOVED***";
		GoogleDrive disk = new GoogleDrive();
		disk.UploadToGoogleDrive(credentialsJson, folderId, qrCodeBytes, fileName);
		NameBook.Text = string.Empty;
		AuthorBook.Text = string.Empty;
		TypeBook.Text = string.Empty;
	}
}