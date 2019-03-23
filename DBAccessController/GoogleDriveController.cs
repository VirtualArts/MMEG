using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers
{
    public class GoogleDriveController
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json
        static string[] Scopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "Drive API .NET";
        static UserCredential credential;
        static DriveService drive;
        static Thread thGetFiles;
        static List<Google.Apis.Drive.v3.Data.File> listFiles = new List<Google.Apis.Drive.v3.Data.File>();
        static List<string> listStringFiles = new List<string>();
        public static void Main(string[] args)
        {
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes, "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Credential file saved to: " + credPath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            bool find = Task.Run(() => FindFiles()).Result;


            Console.Read();
        }

        public static bool UploadFile(string filePath)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = "photo.jpg"
                    };
                    FilesResource.CreateMediaUpload request;
                    using (var stream = new FileStream("files/photo.jpg", FileMode.Open))
                    {
                        //request = driveService.Files.Create(fileMetadata, stream, "image/jpeg");
                        //request.Fields = "id";
                      //  request.Upload();
                        stream.Close();
                        stream.Dispose();
                    }
                   // var file = request.ResponseBody;
                  //  Console.WriteLine("File ID: " + file.Id);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static string GetDriveObjectParentPath(DriveService drive, string objectId, bool digging = false)
        {
            string parentPath = "";

            FilesResource.GetRequest request = drive.Files.Get(objectId);
            request.Fields = "id, name, parents";
            Google.Apis.Drive.v3.Data.File driveObject = request.Execute();

            if (digging)
                parentPath += "/" + driveObject.Name;

            if (driveObject.Parents != null)
                parentPath = GetDriveObjectParentPath(drive, driveObject.Parents[0], true) + parentPath;

            return parentPath;
        }

        private static bool FindFiles()
        {
            //Setup the API drive service
            drive = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential, ApplicationName = System.AppDomain.CurrentDomain.FriendlyName,
            });

            //Setup the parameters of the request
            FilesResource.ListRequest request = drive.Files.List();
            request.PageSize = 1000;
            request.Fields = "nextPageToken, files(mimeType, id, name, parents)";

            //List first 10 files
            IList<Google.Apis.Drive.v3.Data.File> files = request.Execute().Files;
            if (files != null && files.Count > 0)
            {
                foreach (Google.Apis.Drive.v3.Data.File file in files)
                {
                    string parentPath = GetDriveObjectParentPath(drive, file.Id);
                    listFiles.Add(file);
                    listStringFiles.Add(parentPath + file.Name + '\n' + " File Id: " + file.Id);
                   
                    //Console.Write(parentPath + file.Name);
                    //Console.ForegroundColor = ConsoleColor.DarkGray;
                    //Console.WriteLine(" File Id: " + file.Id);
                    //Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else
                Console.WriteLine("No files found.");

            Console.WriteLine("Finished!");
            return true;
        }

        private static bool DownloadFile(string fileId)
        {
            try
            {
                var request = drive.Files.Get(fileId);
                var stream = new System.IO.MemoryStream();
                bool result = false;
                // Add a handler which will be notified on progress changes.
                // It will notify on each chunk download and when the
                // download is completed or failed.
                request.MediaDownloader.ProgressChanged += (IDownloadProgress progress)
                    =>
                {
                    switch (progress.Status)
                    {
                        case DownloadStatus.Downloading:
                            {
                                Console.WriteLine(progress.BytesDownloaded);
                                break;
                            }
                        case DownloadStatus.Completed:
                            {
                                Console.WriteLine("Download complete.");
                                result = true;
                                break;
                            }
                        case DownloadStatus.Failed:
                            {
                                Console.WriteLine("Download failed.");
                                break;
                            }
                    }
                };
                request.Download(stream);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
