using Hola.Api.Configurations;
using Hola.Api.Model;
using Hola.Api.Service.UploadFile.Model;
using Hola.Core.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Options;
using Hola.Core.Common;
using Hola.Core.Helper;
using Hola.GoogleCloudStorage;
using StackExchange.Redis;
using static System.Net.WebRequestMethods;

namespace Hola.Api.Service.UploadFile
{
    public class UploadFileService : BaseUploadFileService, IUploadFileService
    {
        private IConfiguration _configuration;
        private readonly ILogger<UploadFileService> _logger;
        private ServerFolder _serverFolder = new ServerFolder();
        private readonly IOptions<SettingModel> _options;
        private readonly string database = Constant.COMMOND_DB;
        public readonly List<string> _imageExtension = new List<string>() { ".pdf", ".jpeg", ".jpg", ".png" };
        public readonly IUploadFileGoogleCloudStorage _GoogleCloudStorage;

        public UploadFileService(IWebHostEnvironment value, IConfiguration configuration,
            ILogger<UploadFileService> logger, IOptions<SettingModel> options,
            IUploadFileGoogleCloudStorage googleCloudStorage)
        {
            SetWebHostEnvironment(value);
            _configuration = configuration;
            _configuration.GetSection("ServerFolder").Bind(_serverFolder);
            _logger = logger;
            _options = options;
            _GoogleCloudStorage = googleCloudStorage;
        }
        public void SettingEvironmentHttpContext(HttpContext value)
        {
            SetHttpContext(value);
        }
        private async Task<string> UploadFileAsync(IFormFile file, string folder)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            if (folder == null)
                throw new ArgumentNullException(nameof(folder));
            var filePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(filePath))
                await file.CopyToAsync(stream);
            string name = DateTime.UtcNow.ToString("ssddMMyyyy") + file.FileName;
            string url = _GoogleCloudStorage.UploadImage(filePath, folder, name, "credentials.json");
            return await Task.FromResult(url);

        }

        private bool ValidateFileSize(string fileName, Stream fileData, out string error)
        {
            error = string.Empty;
            var ext = Path.GetExtension(fileName).ToLower();
            var fileExtensions = new List<string>() { ".pdf", ".jpeg", ".jpg", ".doc", ".docx", ".png", ".csv", ".txt" };

            if (!fileExtensions.Contains(ext))
            {
                error = ErrorCodes.WrongFileExtension;
                return false;
            }

            if (fileData.Length > 10 * 1024 * 1024) // greater than 10MB
            {
                error = ErrorCodes.MaxFileSizeExceeded;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Upload file to wwwroot/weelpay/Image
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<string> UploadImage(IFormFile file)
        {
            return await UploadFileAsync(file, "weelpay");
        }
        /// <summary>
        /// Upload file to wwwroot/weelpay/Documents
        /// </summary>
        /// <param name="file_doc"></param>
        /// <returns></returns>
        public async Task<string> UploadDocument(IFormFile file_doc)
        {
            return await UploadFileAsync(file_doc, _serverFolder.OriginalPathDocuments);
        }
        /// <summary>
        /// Upload File To New Folder 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="newFolderName"></param>
        /// <returns></returns>
        public async Task<string> UploadFile(IFormFile file, string newFolderName)
        {
            return await UploadFileAsync(file, newFolderName);
        }
        /// <summary>
        /// Upload multiple To New Folder
        /// </summary>
        /// <param name="Files"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public async Task<List<FileItem>> UploadFiles(List<IFormFile> Files, string folder)
        {
            try
            {
                var httpRequest = GetHttpContext().Request;
                List<FileItem> fileResponse = new List<FileItem>();
                foreach (var item in Files)
                {
                    if (item.Length > 0)
                    {
                        var url = await UploadFileAsync(item, folder);
                        fileResponse.Add(new FileItem { FileName = item.FileName, Url = url });
                    }
                }
                return await Task.FromResult(fileResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public Task<FileModel> UploadFile(ClaimsPrincipal User, string fileName, Stream fileData, out string error)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            var userId = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;

            error = string.Empty;
            if (string.IsNullOrEmpty(fileName))
            {
                error = ErrorCodes.MissingFileName;
                return null;
            }
            if (fileData == null || fileData.Length == 0)
            {
                error = ErrorCodes.MissingFileData;
                return null;
            }
            if (!ValidateFileSize(fileName, fileData, out error)) return null;

            var ext = Path.GetExtension(fileName);
            string sql = string.Format("INSERT INTO cmn.\"Gallery\" " +
                                       "(\"FilePath\", \"FileUrl\", \"Size\", \"Extension\",\"CreatedDate\",\"ModifiedDate\", \"CreatedBy\", \"ModifiedBy\", \"FileName\") " +
                                       "VALUES('{0}', '', {1}, '{2}', '{3}', '{4}', '{5}', '{6}','{7}') RETURNING \"Id\";",
                                       userId, fileData.Length, ext, DateTime.UtcNow, DateTime.UtcNow, userId, userId, fileName);

            var obj = DatabaseHelper.ExcuteSql(sql, setting).Rows;
            long newId = 0;
            if (obj != null && obj.Count > 0) newId = long.Parse(obj[0]["Id"].ToString());

            var filePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(filePath))
            {
                fileData.CopyTo(stream);
            }
            string name = DateTime.UtcNow.ToString("ssddMMyyyy") + fileName;
            // Check content Type : 
            string contentType = null;
            string folder = GetFolder(ext);
            if (folder=="image") contentType = "image/jpeg";
            string url = _GoogleCloudStorage.UploadFile(filePath, 
                userId, name, "credentials.json",folder,contentType);

            sql = string.Format("UPDATE cmn.\"Gallery\"" +
                                "SET \"FileUrl\" = '{0}'" +
                                " WHERE \"Id\" = {1}", url, newId);
            DatabaseHelper.ExcuteNonQuery(sql, setting);
            return Task.FromResult(Get(newId));
        }
        /// <summary>
        /// Build url Return to Customer
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="host"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetFolder(string ext)
        {
            string url = string.Empty;
            var extensions = new List<string>() { ".pdf", ".jpeg", ".jpg", ".png" };
            if (extensions.Contains(ext))
            {
                url = "image";
            }
            else
            {
                url = "document";
            }
            return url;
        }
  
        public List<string> GetListExtensionSupport()
        {
            return new List<string>() { ".pdf", ".jpeg", ".jpg", ".doc", ".docx", ".png", ".csv", ".txt" };
        }

        public FileModel Get(long id)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            string sql = string.Format("SELECT \"Id\",\"FileName\", \"FileUrl\", \"Size\", \"Extension\" FROM cmn.\"Gallery\" WHERE \"Id\"={0}", id);
            setting.Connection += "Database=" + database;
            return DatabaseHelper.ExcuteQueryToList<FileModel>(sql, setting).FirstOrDefault();
        }
    }
}
