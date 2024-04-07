namespace Library;

public partial class SearchResultPage : ContentPage
{
	public SearchResultPage(List<Model.Books> results)
	{
		InitializeComponent();
		ResultsListView.ItemsSource = results;
	}
}