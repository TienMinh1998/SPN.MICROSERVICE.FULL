using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hola.GoogleCloudStorage
{
    public class HolaGoogleStorage : IUploadFileGoogleCloudStorage
    {
        private string _bucketName = "hola-files";
        private readonly string _fileNameSaveImage = "image";
        private readonly string _fileDocument = "document";

        public void SetBucketName(string value)
        {
            _bucketName = value;
        }
        public string UploadImage(string path, string useid, string fileName, string credentials_path)
        {
            try
            {
                string bucketName = _bucketName;
                GoogleCredential credential = null;
                using (var jsonStream = new FileStream(credentials_path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    credential = GoogleCredential.FromStream(jsonStream);
                }
                var storageClient = StorageClient.Create(credential);

                string object_name = $"{_fileNameSaveImage}/{useid}/{fileName}";
                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var response = storageClient.UploadObject(bucketName, $"{_fileNameSaveImage}/{useid}/{fileName}",
                        null, fileStream);
                }
                var urlDownload = GetURL(bucketName, object_name, credentials_path);
                string[] urls = urlDownload.Split('?');
                return urls[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        public string UploadDocument(string path, string useid, string fileName, string credentials_path)
        {
            try
            {
                string bucketName = _bucketName;
                GoogleCredential credential = null;
                using (var jsonStream = new FileStream(credentials_path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    credential = GoogleCredential.FromStream(jsonStream);
                }
                var storageClient = StorageClient.Create(credential);

                string object_name = $"{_fileDocument}/{useid}/{fileName}";
                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var response = storageClient.UploadObject(bucketName, $"{_fileDocument}/{useid}/{fileName}",
                        null, fileStream);
                }
                return GetURL(bucketName, object_name, credentials_path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public string UploadImage(Stream _stream, string useid, string fileName, string credentials_path)
        {
            try
            {
                string bucketName = _bucketName;
                GoogleCredential credential = null;
                using (var jsonStream = new FileStream(credentials_path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    credential = GoogleCredential.FromStream(jsonStream);
                }
                var storageClient = StorageClient.Create(credential);
                FileStream fileStream = _stream as FileStream;
                string object_name = $"{_fileNameSaveImage}/{useid}/{fileName}";

                var response = storageClient.UploadObject(bucketName, $"{_fileNameSaveImage}/{useid}/{fileName}",
                    null, fileStream);

                return GetURL(bucketName, object_name, credentials_path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public string UploadFile(string path, string useId, string fileName, string credentials_path,string folder,string contentType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(folder))
                    throw new ArgumentNullException(nameof(folder));
                if (string.IsNullOrWhiteSpace(useId))
                    throw new ArgumentNullException(nameof(useId));
                if (string.IsNullOrWhiteSpace(fileName))
                    throw new ArgumentNullException(nameof(fileName));
                if (string.IsNullOrWhiteSpace(path))
                    throw new ArgumentNullException(nameof(path));
                string bucketName = _bucketName;
                GoogleCredential credential = null;
                using (var jsonStream = new FileStream(credentials_path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    credential = GoogleCredential.FromStream(jsonStream);
                }
                var storageClient = StorageClient.Create(credential);
                
                string object_name = $"{folder}/{useId}/{fileName}";
                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var response = storageClient.UploadObject(bucketName, $"{folder}/{useId}/{fileName}",
                        contentType, fileStream);
                }
                var urlDownload = GetURL(bucketName, object_name, credentials_path);
                return urlDownload;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        private string GetURL(string bucketName, string objectName, string credentialFilePath)
        {
            try
            {
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credentialFilePath);
                // V4 is the default signing version.
                string url = urlSigner.Sign(bucketName, objectName, TimeSpan.FromHours(1), HttpMethod.Get);
                string[] urls =url.Split('?');
                return urls.First();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }
    }
}
