using Library.Pages.HelperPages;
using Library.Pages.MainPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	}
}
