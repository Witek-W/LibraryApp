using QRCoder;

namespace Library;

public partial class ReturnBookPage : ContentPage
{
	public ReturnBookPage()
	{
		InitializeComponent();
	}
	private void OnGenerateClicked(object sender, EventArgs e)
	{
		QRCodeGenerator qrGenerator = new QRCodeGenerator();
		QRCodeData qrCodeData = qrGenerator.CreateQrCode(InputText.Text, QRCodeGenerator.ECCLevel.H);
		PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
		byte[] qrCodeBytes = qRCode.GetGraphic(20);
		QrCodeImage.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));	
	}
}