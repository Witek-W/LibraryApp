using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSApi.Api;

namespace Library.Model
{
    internal class ApiSms
    {
		public async Task SendSmsToReader(string Text, string Number)
		{
			//IClient client = new ClientOAuth("***REMOVED***");
			//var smsApi = new SMSFactory(client, new ProxyHTTP("https://api.smsapi.pl/"));
			//var result = smsApi.ActionSend()
			//	.SetText(Text)
			//	.SetTo(Number)
			//	.Execute();
		}
	}
}
