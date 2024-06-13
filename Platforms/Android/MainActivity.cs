using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.NFC;

namespace Library
{
	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
	public class MainActivity : MauiAppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			CrossNFC.Init(this);
		}

		protected override void OnResume()
		{
			base.OnResume();
			CrossNFC.OnResume();
		}

		protected override void OnNewIntent(Android.Content.Intent intent)
		{
			base.OnNewIntent(intent);
			CrossNFC.OnNewIntent(intent);
		}
	}
}
