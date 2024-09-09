using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views.InputMethods;
using AndroidX.AppCompat.App;
using Library.Model;
using Library.Pages.ManageLibraryPages;
using Plugin.NFC;
using System.ComponentModel;

namespace Library
{
	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | 
		ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : MauiAppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			CrossNFC.Init(this);
			AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
			base.OnCreate(savedInstanceState);
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
		protected override void OnPause()
		{
			base.OnPause();
		}
	}
}
