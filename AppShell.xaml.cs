namespace Library
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();

			Routing.RegisterRoute(nameof(CheckBookPage),typeof(CheckBookPage));
		}
	}
}
