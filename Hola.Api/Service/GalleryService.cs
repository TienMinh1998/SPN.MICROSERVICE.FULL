using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Hola.Api.Model;
using Hola.Core.Common;
using Hola.Core.Helper;
using Hola.Core.Model;
using Hola.Core.Service;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hola.Api.Service
{
    public class GalleryService : BaseService
    {
        private readonly IOptions<SettingModel> _options;
        private readonly string database = Constant.COMMOND_DB;

        public GalleryService(IOptions<SettingModel> setting) : base(setting)
        {
            _options = setting;
        }

        public ServiceConfig GetBlobSettings()
        {
            var setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            string command = string.Empty;
            string command1 = string.Empty;
            setting.Connection += "Database=" + database;
            command = string.Format("SELECT * FROM cmn.\"ServiceConfig\" " +
                                    "WHERE \"ServiceId\" = 1");
            DataTable settings = DatabaseHelper.ExcuteSql(command, setting);
            command1 = string.Format("SELECT \"Name\",\"Endpoint\" FROM cmn.\"Service\"");
            List<Dictionary<string, object>> services = new List<Dictionary<string, object>>();
            services = DatabaseHelper.ExcuteQueryToDict(command1, setting);

            var config = new ServiceConfig();
            config.Services = new Dictionary<string, string>();

            foreach (var itemDictionary in services.ToArray())
            {
                var _key = itemDictionary.Values.FirstOrDefault();
                var _value = itemDictionary.Values.LastOrDefault();
                config.Services.Add(_key.ToString(), _value.ToString());
            }

            foreach (DataRow item in settings.Rows)
            {
                if (item[2].ToString() == "Blob.EnvFolder")
                {
                    config.BlobSettings.EnvFolder = item[3].ToString();
                }

                if (item[2].ToString() == "Blob.AccessToken")
                {
                    config.BlobSettings.AccessToken = item[3].ToString();
                }

                if (item[2].ToString() == "Blob.SubFolder")
                {
                    config.BlobSettings.SubFolder = item[3].ToString();
                }
            }

            Resolver.Register(config);
            return config;
        }

        private bool ValidateFileSize(string fileName, Stream fileData, out string error)
        {
            error = string.Empty;
            var ext = Path.GetExtension(fileName).ToLower();
            var fileExtensions = new List<string>() { ".pdf", ".jpeg", ".jpg", ".doc", ".docx", ".png", ".csv" };

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

        public async Task<string> UploadFileToAzure(ClaimsPrincipal user, string filename, Stream fileStream)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };

            setting.Connection += "Database=" + database;
            string userId = user.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
            string error = string.Empty;

            if (string.IsNullOrEmpty(filename))
            {
                return ErrorCodes.MissingFileName;
            }
            if (fileStream == null || fileStream.Length == 0)
            {
                return ErrorCodes.MissingFileData;
            }
            if (!ValidateFileSize(filename, fileStream, out error))
            {
                return error;
            }

            var ext = Path.GetExtension(filename);
            var config = GetBlobSettings();
            var path = Path.Combine(config.BlobSettings.SubFolder) + "/";

            string sqlGetNextVal = "SELECT ((select \"Id\" from cmn.\"Gallery\" order by \"Id\" desc limit 1) + i.inc) as \"NewId\" " +
                "FROM cmn.\"Galery_Id_seq\", (SELECT seqincrement AS inc FROM pg_sequence WHERE seqrelid = 'cmn.\"Galery_Id_seq\"'::regclass::oid) AS i;";

            var obj = DatabaseHelper.ExcuteSql(sqlGetNextVal, setting).Rows;

            long newId = 0;
            if (obj != null && obj.Count > 0)
            {
                newId = long.Parse(obj[0]["NewId"].ToString());
            }

            var container = new BlobContainerClient(config.BlobSettings.AccessToken, config.BlobSettings.EnvFolder);
            await container.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
            path = Path.Combine(path, $"{newId}{ext}");
            BlobClient blob = container.GetBlobClient(path);
            await blob.UploadAsync(fileStream);
            string fileUri = blob.Uri.AbsoluteUri;

            string sql = string.Format("INSERT INTO cmn.\"Gallery\" " +
                                       "(\"FilePath\", \"FileUrl\", \"Size\", \"Extension\",\"CreatedDate\",\"ModifiedDate\", \"CreatedBy\", \"ModifiedBy\", \"FileName\") " +
                                       "VALUES('{0}', '{1}', '{2}', '{3}', now(), 'now()', '{4}', '{5}', '{6}') RETURNING \"Id\";",
                                       path, fileUri, fileStream.Length, ext, userId, userId, filename);

            var newRecord = DatabaseHelper.ExcuteSql(sql, setting).Rows;

            long newRecordId = 0;
            if (newRecord != null && newRecord.Count > 0)
            {
                newRecordId = long.Parse(newRecord[0]["Id"].ToString());
            }

            return newRecordId > 0 ? fileUri : string.Empty;
        }

        public FileModel Get(long id)
        {
            string sql = string.Format("SELECT \"Id\",\"FileName\", \"FileUrl\", \"Size\", \"Extension\" FROM cmn.\"Gallery\" WHERE \"Id\"={0}", id);
            var response = FirstOrDefault<FileModel>(rawConnection + Constant.COMMOND_DB, sql);
            if (response != null)
                return response;
            return null;
        }
        public JsonResponseModel GetFiles(int[] filesid)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            try
            {
                string _listID = string.Empty;
                foreach (var item in filesid) _listID += item + ",";
                _listID += "-1";
                string sql = string.Format("SELECT \"Id\",\"FileName\", \"FileUrl\", \"Size\", \"Extension\" FROM cmn.\"Gallery\" WHERE \"Id\" IN ({0})", _listID);
                setting.Connection += "Database=" + database;
                var response = DatabaseHelper.ExcuteQueryToList<FileModelResponse>(sql, setting);
                if (response == null) JsonResponseModel.Success(new List<FileModelResponse>());
                return JsonResponseModel.Success(response);
            }
            catch (Exception ex)
            {
                return JsonResponseModel.Error(ex.Message, SystemParam.SERVER_ERROR_CODE);
            }



        }
        public FileModel UploadFile(ClaimsPrincipal User, string fileName, Stream fileData, out string error)
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
            if (!ValidateFileSize(fileName, fileData, out error))
            {
                return null;
            }

            var ext = Path.GetExtension(fileName);
            var config = GetBlobSettings();
            var path = Path.Combine(config.BlobSettings.SubFolder);


            string sql = string.Format("INSERT INTO cmn.\"Gallery\" " +
                                       "(\"FilePath\", \"FileUrl\", \"Size\", \"Extension\",\"CreatedDate\",\"ModifiedDate\", \"CreatedBy\", \"ModifiedBy\", \"FileName\") " +
                                       "VALUES('{0}', '', {1}, '{2}', '{3}', '{4}', '{5}', '{6}','{7}') RETURNING \"Id\";", path, fileData.Length, ext, DateTime.UtcNow, DateTime.UtcNow, userId, userId, fileName);
            var obj = DatabaseHelper.ExcuteSql(sql, setting).Rows;
            long newId = 0;
            if (obj != null && obj.Count > 0)
            {
                newId = long.Parse(obj[0]["Id"].ToString());
            }
            var container = new BlobContainerClient(config.BlobSettings.AccessToken, config.BlobSettings.EnvFolder);
            container.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
            if (!container.Exists().Value)
            {
                container.CreateIfNotExists();
            }

            path = Path.Combine(path, $"{newId}{ext}");
            BlobClient blob = container.GetBlobClient(path);

            //blob.DeleteIfExistsAsync();
            blob.UploadAsync(fileData);

            var fileUri = blob.Uri.AbsoluteUri;
            sql = string.Format("UPDATE cmn.\"Gallery\"" +
                                "SET \"FileUrl\" = '{0}'" +
                                " WHERE \"Id\" = {1}", fileUri, newId);

            DatabaseHelper.ExcuteNonQuery(sql, setting);
            return Get(newId);
        }
        public bool UploadFile(long id, string fileName, Stream fileData, ClaimsPrincipal User, out string error)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            error = string.Empty;
            var userId = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
            if (string.IsNullOrEmpty(fileName))
            {
                error = ErrorCodes.MissingFileName;
                return false;
            }
            if (fileData == null || fileData.Length == 0)
            {
                error = ErrorCodes.MissingFileData;
                return false;
            }
            if (!ValidateFileSize(fileName, fileData, out error))
            {
                return false;
            }
            var config = GetBlobSettings();
            var path = Path.Combine(config.BlobSettings.SubFolder);
            var old = Get(id);
            var container = new BlobContainerClient(config.BlobSettings.AccessToken, config.BlobSettings.EnvFolder);
            container.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
            if (!container.Exists().Value)
            {
                container.CreateIfNotExists();
            }
            var path1 = Path.Combine(path, $"{old.Id}{old.Extension}");
            BlobClient blobOld = container.GetBlobClient(path1);
            blobOld.DeleteIfExistsAsync();
            var ext = Path.GetExtension(fileName);

            var sql = string.Format("UPDATE cmn.\"Gallery\"" +
            "SET \"FilePath\" = '{0}',\"Size\" = {1}," +
                "\"Extension\" = '{2}',\"ModifiedDate\" = '{3}'," +
                "\"ModifiedBy\" = '{4}', \"FileName\" = '{6}' WHERE \"Id\" = {5};", path, fileData.Length, ext, DateTime.UtcNow, userId, id, fileName);

            DatabaseHelper.ExcuteNonQuery(sql, setting);

            path1 = Path.Combine(path, $"{id}{ext}");
            BlobClient blobNew = container.GetBlobClient(path1);
            //blobNew.DeleteIfExistsAsync();
            blobNew.UploadAsync(fileData);

            var fileUri = blobNew.Uri.AbsoluteUri;

            sql = string.Format("UPDATE cmn.\"Gallery\"" +
                                "SET \"FileUrl\" = '{0}'" +
                                " WHERE \"Id\" = {1};", fileUri, id);
            DatabaseHelper.ExcuteNonQuery(sql, setting);
            return true;
        }
    }
}