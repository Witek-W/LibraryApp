namespace Library.Pages.HelperPages;

public partial class NoRecordError : ContentPage
{
	public NoRecordError()
	{
		InitializeComponent();
	}
	private void ReturnButton(object sender, EventArgs e)
	{
		MainPage BrandNew = new MainPage();
		NavigationPage.SetHasBackButton(BrandNew, false);
		Navigation.PushAsync(BrandNew);
	}
}