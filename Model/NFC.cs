using Library.Pages.ManageLibraryPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;
using static System.Net.Mime.MediaTypeNames;

namespace Library.Model
{
	
    class NFC
    {
		public event EventHandler<string> MessageReceived;
		public event EventHandler<int> MessagePublished;
		public NFCNdefTypeFormat _type;
		private int ID;
		private readonly INavigation _navigation;
		public NFC(INavigation navigation)
		{
			_navigation = navigation;
			CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
			CrossNFC.Current.OnTagDiscovered += Current_OnTagDiscovered;
			CrossNFC.Current.OnMessagePublished += Current_OnMessagePublished;
		}
		//Read NFC
		private void Current_OnMessageReceived(ITagInfo tagInfo)
		{
			if (tagInfo == null || tagInfo.Records == null || tagInfo.Records.Length == 0)
				return;

			foreach (var record in tagInfo.Records)
			{
				if (record.TypeFormat == NFCNdefTypeFormat.WellKnown)
				{
					var text = GetTextFromPayload(record.Payload);
					MessageReceived?.Invoke(this, text);
				}
			}
		}
		private void Current_OnMessagePublished(ITagInfo tagInfo)
		{
			//StopWriteNfcTag();
			MessagePublished?.Invoke(this, ID);
		}
		private async void Current_OnTagDiscovered(ITagInfo tagInfo, bool format)
		{
			if (!CrossNFC.Current.IsWritingTagSupported)
			{
				return;
			}

			try
			{

				if (ID != 0)
				{
					NFCNdefRecord record = new NFCNdefRecord
					{
						TypeFormat = NFCNdefTypeFormat.WellKnown,
						Payload = NFCUtils.EncodeToByteArray(ID.ToString()),
					};
					tagInfo.Records = new[] { record };
				
					CrossNFC.Current.PublishMessage(tagInfo);
				}
			} catch(Exception ex)
			{
				StopWriteNfcTag();
				await GoToMainPageAsync();
			}
		}
		private async Task GoToMainPageAsync()
		{
			MainPage refreshMainPage = new MainPage();
			NavigationPage.SetHasBackButton(refreshMainPage, false);
			await _navigation.PushAsync(refreshMainPage);
		}
		private string GetTextFromPayload(byte[] payload)
		{
			var statusByte = payload[0];
			var isUtf16 = (statusByte & 0x80) != 0;
			var languageCodeLength = statusByte & 0x3F;
			var textEncoding = isUtf16 ? Encoding.BigEndianUnicode : Encoding.UTF8;

			return textEncoding.GetString(payload, languageCodeLength + 1, payload.Length - languageCodeLength - 1);
		}
		public void ReadNfcTag()
		{
			CrossNFC.Current.StartListening();
		}
		public void StopReadNfcTag()
		{
			CrossNFC.Current.StopListening();
		}
		//Write NFC
		public void WriteNfcTag(int id)
		{
			ID = id;
			CrossNFC.Current.StartListening();
			CrossNFC.Current.StartPublishing();
		}
		public void StopWriteNfcTag()
		{
			CrossNFC.Current.StopListening();
			CrossNFC.Current.StopPublishing();
		}
	}
}
