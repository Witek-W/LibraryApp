using Library.Pages.HelperPages;
using Library.Pages.MainPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library.Model
{
	internal class Helpers
	{
		private readonly INavigation _navigation;

		public Helpers(INavigation navigation)
		{
			_navigation = navigation;
		}
		//InternetError
		public void ShowInternetError()
		{
			ErrorStartPage BrandNew = new ErrorStartPage();
			NavigationPage.SetHasBackButton(BrandNew, false);
			_navigation.PushAsync(BrandNew);
		}
		//SqlError
		public void ShowSQLError()
		{
			NoRecordError BrandNew = new NoRecordError();
			NavigationPage.SetHasBackButton(BrandNew, false);
			_navigation.PushAsync(BrandNew);
		}
		//SMSApiError
		public void ShowSMSApiError()
		{
			SmsApiError BrandNew = new SmsApiError();
			NavigationPage.SetHasBackButton(BrandNew, false);
			_navigation.PushAsync(BrandNew);
		}
		//Regex dla walidacji uzytkownika
		//Regex
		public bool CheckString(string input, int variant)
		{
			switch (variant)
			{
				//Sprawdzanie czy string ma liczby i znaki specjalnie z wyłączeniem "-" i spacji
				case 1:
					var regexNumbers = new Regex(@"[^a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ\s\-]");
					if (regexNumbers.IsMatch(input)) return true;
					return false;
				//Sprawdzanie znaków specjalnych
				case 2:
					var regexSpecial = new Regex(@"[^\w]");
					if (regexSpecial.IsMatch(input)) return true;
					return false;
				//Sprawdzanie czy ma liczby i znaki specjalne
				case 3:
					var regexNumberSpecial = new Regex(@"[\d\W]");
					if (regexNumberSpecial.IsMatch(input)) return true;
					return false;
				//Sprawdzanie czy ma znaki i litery
				case 4:
					foreach (char c in input)
					{
						if (!char.IsDigit(c))
						{
							return true;
						}
					}
					return false;
				default:
					return true;
			}
		}
	}
}
