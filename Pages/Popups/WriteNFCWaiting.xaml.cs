using Library.Pages.ManageLibraryPages;

namespace Library.Pages.Popups;

public partial class WriteNFCWaiting : ContentPage
{
	public WriteNFCWaiting()
	{
		InitializeComponent();
		this.Loaded += Page_Loaded;
	}
	void Page_Loaded(object sender, EventArgs e)
	{
		this.Loaded -= Page_Loaded;
		PoppingIn();
	}
	public void PoppingIn()
	{
		var contentSize = this.Content.Measure(Window.Width, Window.Height, MeasureFlags.IncludeMargins);
		var contentHeight = contentSize.Request.Height;

		this.Content.TranslationY = contentHeight;

		this.Animate("Background",
			callback: v => this.Background = new SolidColorBrush(Colors.Black.WithAlpha((float)v)),
			start: 0d,
			end: 0.7d,
			rate: 32,
			length: 350,
			easing: Easing.CubicOut,
			finished: (v, k) => this.Background = new SolidColorBrush(Colors.Black.WithAlpha(0.7f)));
		this.Animate("Content",
			callback: v => this.Content.TranslationY = (int)(contentHeight - v),
			start: 0,
			end: contentHeight,
			length: 500,
			easing: Easing.CubicInOut,
			finished: (v, k) => this.Content.TranslationY = 0);
	}
	public Task PoppingOut()
	{
		var done = new TaskCompletionSource();
		
		var contentSize = this.Content.Measure(Window.Width, Window.Height, MeasureFlags.IncludeMargins);
		var windowHeight = contentSize.Request.Height;
		
		this.Animate("Background",
			callback: v => this.Background = new SolidColorBrush(Colors.Black.WithAlpha((float)v)),
			start: 0.7d,
			end: 0d,
			rate: 32,
			length: 350,
			easing: Easing.CubicIn,
			finished: (v, k) => this.Background = new SolidColorBrush(Colors.Black.WithAlpha(0.0f)));
		
		this.Animate("Content",
			callback: v => this.Content.TranslationY = (int)(windowHeight - v),
			start: windowHeight,
			end: 0,
			length: 500,
			easing: Easing.CubicInOut,
			finished: (v, k) =>
			{
				this.Content.TranslationY = windowHeight;
				done.TrySetResult();
			});
		
		return done.Task;
	}
	public async Task Close()
	{
		await PoppingOut();
		await Navigation.PopModalAsync(animated: false);
	}
	public async Task Change()
	{
		NFCScanning.RepeatCount = 0;
		NFCScanning.IsVisible = false;
		NfcLabel.IsVisible = false;
		NFCConfirm.IsVisible = true;
		await Task.Delay(1000);
		await Close();
	}
}