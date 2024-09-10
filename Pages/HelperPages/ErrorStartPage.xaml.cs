namespace Library.Pages.MainPages;

public partial class ErrorStartPage : ContentPage
{
	public ErrorStartPage()
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