namespace Library.Pages.Popups;

public partial class Loading : ContentPage
{
	public Loading()
	{
		InitializeComponent();
	}
	public async Task Close()
	{
		await Navigation.PopModalAsync(animated: false);
	}
}