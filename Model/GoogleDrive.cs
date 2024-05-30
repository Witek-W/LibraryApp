using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
	public class GoogleDrive
	{
		public void UploadToGoogleDrive(string credentialsPath, string folderID, byte[] file, string filename)
		{
			GoogleCredential credential;
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(credentialsPath)))
			{
				credential = GoogleCredential.FromStream(stream).CreateScoped(new[]
				{
					DriveService.ScopeConstants.DriveFile
				});

				var service = new DriveService(new BaseClientService.Initializer()
				{
					HttpClientInitializer = credential,
					ApplicationName = "Google Drive Upload QR"
				});


				var fileMetaData = new Google.Apis.Drive.v3.Data.File()
				{
					Name = filename,
					Parents = new List<string> { folderID }
				};

				FilesResource.CreateMediaUpload requestl;
				using (var stream2 = new MemoryStream(file))
				{
					requestl = service.Files.Create(fileMetaData, stream2, "");
					requestl.Fields = "id";
					requestl.Upload();
				}
			}
		}
	}
}
