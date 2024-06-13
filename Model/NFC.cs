using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace Library.Model
{
    class NFC
    {
		public event EventHandler<string> MessageReceived;
		public event EventHandler<string> MessagePublished;
		public const string MIME_TYPE = "application/com.companyname.library";
		public NFCNdefTypeFormat _type;

		public NFC()
		{
			CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
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
		
	}
}
