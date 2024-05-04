using Library.Model;
using System.Linq;
using System.Xml;

namespace Library.Pages.MainPages;

public partial class NotificationPage : ContentPage
{
	public NotificationPage(IQueryable<BookWithReaderInfo> results)
	{
		InitializeComponent();
		ResultsListView.ItemsSource = results.ToList();
		
	}
}