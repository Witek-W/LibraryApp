namespace Library.Pages.HelperPages;

public partial class SmsApiError : ContentPage
{
	public SmsApiError()
	{
		InitializeComponent();
	}
	private void TryMainPage(object sender, EventArgs e)
	{
		MainPage BrandNew = new MainPage();
		NavigationPage.SetHasBackButton(BrandNew, false);
		Navigation.PushAsync(BrandNew);
	}
}