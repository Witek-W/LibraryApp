using epj.Expander.Maui;
using Library.Model;
using System.Globalization;

namespace Library;

public partial class SearchResultPage : ContentPage
{
	private int currentPage = 0;
	private const int pageSize = 9;
	private List<Model.Books> allbooks;
	public SearchResultPage(List<Model.Books> results)
	{
		InitializeComponent();
		allbooks = results;
		LoadPage(currentPage);
		PageNumberLabel.Text = (currentPage + 1).ToString();
	}

	private void LoadPage(int pageNumber)
	{
		var booksForPage = allbooks.Skip(pageNumber * pageSize).Take(pageSize).ToList();
		ResultsListView.ItemsSource = booksForPage;
		currentPage = pageNumber;
	}
	public void NextPage(object sender, EventArgs e)
	{
		LoadPage(currentPage + 1);
		PageNumberLabel.Text = (currentPage + 1).ToString();
	}
	public void PreviousPage(object sender, EventArgs e)
	{
		if(currentPage > 0)
		{
			LoadPage(currentPage - 1);
			PageNumberLabel.Text = (currentPage + 1).ToString();
		}
	}

	private void TestExpander(object sender, EventArgs e)
	{
		var expander = (Expander)sender;
		expander.IsExpanded = !expander.IsExpanded;
	}
}