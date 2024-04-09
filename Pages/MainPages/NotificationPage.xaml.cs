using Library.Model;
using System.Linq;

namespace Library.Pages.MainPages;

public partial class NotificationPage : ContentPage
{
	public NotificationPage(IQueryable<Books> results)
	{
		InitializeComponent();
		ResultsListView.ItemsSource = results.ToList();
	}
}