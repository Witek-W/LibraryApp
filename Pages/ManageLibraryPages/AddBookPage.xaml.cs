
using Library.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls.PlatformConfiguration;
using QRCoder;
using System.Text;
namespace Library;

public partial class AddBookPage : ContentPage
{
	private Helpers _help;
	public AddBookPage()
	{
		InitializeComponent();
		AddBookButton.IsEnabled = false;
		_help = new Helpers(Navigation);
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
		string IDrecord = "";
		try
		{
			using (var dbContext = new LibraryDbContext())
			{
				var newRecord = new Books { Name = name, Author = author, Type = type, Availability = 1, Rental_date = null, Planned_return_date = null, ReaderID = null };
				dbContext.Book.Add(newRecord);
				dbContext.SaveChanges();
				IDrecord = newRecord.Id.ToString();
			}
		}
		catch
		{
			_help.ShowInternetError();
			return;
		}
		QRCodeGenerator qrGenerator = new QRCodeGenerator();
		QRCodeData qrCodeData = qrGenerator.CreateQrCode(IDrecord, QRCodeGenerator.ECCLevel.H);
		PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
		byte[] qrCodeBytes = qRCode.GetGraphic(10);
		
		string fileName = $"kodQR_{name}_{author}_{type}_{IDrecord}.png";
		//Problem ze znalezieniem pliku, rozwi¹zanie chwilowe
		string credentialsJson = @"{
		""type"": ""***REMOVED***"",
		""project_id"": ""***REMOVED***"",
		""private_key_id"": ""***REMOVED***"",
		""private_key"": ""***REMOVED***"",
		""client_email"": ""***REMOVED***"",
		""client_id"": ""***REMOVED***"",
		""auth_uri"": ""***REMOVED***"",
		""token_uri"": ""***REMOVED***"",
		""auth_provider_x509_cert_url"": ""***REMOVED***"",
		""client_x509_cert_url"": ""***REMOVED***""}";
		string folderId = "***REMOVED***";
		GoogleDrive disk = new GoogleDrive();
		disk.UploadToGoogleDrive(credentialsJson, folderId, qrCodeBytes, fileName);
		await DisplayAlert("Powiadomienie", "Ksi¹¿ka dodana pomyœlnie", "Wygeneruj kod QR");
		NameBook.Text = string.Empty;
		AuthorBook.Text = string.Empty;
		TypeBook.Text = string.Empty;
	}
}