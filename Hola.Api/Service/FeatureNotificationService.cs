using Hola.Api.Models;
using Hola.Core.Common;
using Hola.Core.Helper;
using Hola.Core.Model;
using Hola.Core.Model.CommonModel;
using Hola.Core.Service;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hola.Api.Service
{
    public class FeatureNotificationService
    {
        private readonly IOptions<SettingModel> _options;
        private readonly string database = Constant.MESSAGE_DB;

        public FeatureNotificationService(IOptions<SettingModel> setting)
        {
            _options = setting;
        }

        /// <summary>
        /// LVTan
        /// Check Exists FeatureNotification
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool CheckExists(long id, string title)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };

            string sql = "SELECT COUNT(*) FROM msg.\"FeatureNotification\" n";

            if (id > 0)
            {
                sql += String.Format("WHERE n.\"IsDeleted\" = '0' AND (CASE WHEN (n.\"Id\" != {0} AND Trim(n.\"Title\") = Trim({1})) THEN 1 ELSE 0 END = 1)", id, title);
            }
            else
            {
                sql += String.Format("WHERE n.\"IsDeleted\" = '0' AND (CASE WHEN (Trim(n.\"Title\") = Trim({0})) THEN 1 ELSE 0 END = 1)", title);
            }

            setting.Connection += "Database=" + database;

            int result = DatabaseHelper.ExcuteQueryToList<int>(sql, setting).FirstOrDefault();

            return result > 0;
        }

        public async Task<List<KeyValuePair<string, long>>> SendToUser(long id, short languageId, string token)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            List<string> userIds = new List<string>();
            APICrossHelper aPICrossHelper = new APICrossHelper();
            var result = await aPICrossHelper.Get<JsonResponseModel>(_options.Value.UserServiceUrl + "/User/GetUserIdIsUserByLanguageId?languageId=" + languageId, token);
            if (result.Data != null)
            {
                userIds = JsonConvert.DeserializeObject<List<string>>(result.Data.ToString());
            }
            List<KeyValuePair<string, long>> featureList = new List<KeyValuePair<string, long>>();
            foreach (var a in userIds)
            {
                featureList.Add(KeyValuePair.Create(a, id));
            }
            PingService.AddFeature(featureList);
            string sql = string.Format("UPDATE msg.\"FeatureNotification\" " +
                "SET \"IsSend\" = CAST(1 AS BIT) " +
                "WHERE \"Id\" = {0}", id);
            DatabaseHelper.ExcuteNonQuery(sql, setting);
            return featureList;
        }

        /// <summary>
        /// LVTan
        /// Seach FeatureNotification
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public DataResultsModel Search(FeatureNotificationFilterModel filters)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };

            string conditions = string.Empty;
            string orderBy = string.Empty;
            string paging = string.Empty;

            if (!string.IsNullOrEmpty(filters.SearchText))
            {
                conditions += "AND (n.\"Title\" ILIKE '%" + filters.SearchText + "%' " +
                              "OR n.\"ShortDescription\" ILIKE '%" + filters.SearchText + "%' " +
                              "OR n.\"Description\" ILIKE '%" + filters.SearchText + "%' " +
                              "OR n.\"CreateBy\" ILIKE '%" + filters.SearchText + "%' " +
                              "OR n.\"ModifiedBy\" ILIKE '%" + filters.SearchText + "%' " +
                              "OR CAST(n.\"CreateDate\" AS VARCHAR(10)) ILIKE  '%" + filters.SearchText + "%' " +
                              "OR CAST(n.\"ModifiedDate\" AS VARCHAR(10)) ILIKE  '%" + filters.SearchText + "%') ";
            }

            if (filters.LanguageId.HasValue && filters.LanguageId.Value > 0)
            {
                conditions += " AND n.\"LanguageId\" = " + filters.LanguageId.Value;
            }

            if (!string.IsNullOrEmpty(filters.OrderBy))
            {
                orderBy += " order by \"" + filters.OrderBy + "\" " + (string.IsNullOrEmpty(filters.OrderDirection) ? string.Empty : filters.OrderDirection);
            }
            else
            {
                orderBy += $" order by \"CreateDate\" DESC";
            }

            var take = !filters.PageSize.HasValue || filters.PageSize.Value <= 0 ? "" : $"FETCH NEXT " + filters.PageSize.Value + " ROWS ONLY";
            paging += " OFFSET " + ((filters.PageNumber ?? 1) - 1) * (filters.PageSize ?? 0) + " ROWS " + take;

            string totalCountQuery = string.Format("SELECT COUNT(1) FROM msg.\"FeatureNotification\" n WHERE n.\"IsDeleted\" = '0' {0}", conditions);

            string sql = string.Format("SELECT \"Id\", \"Title\", \"ShortDescription\", \"Description\", \"ImageUrl\", \"CreateDate\", \"CreateBy\", " +
                                        "\"ModifiedDate\", \"ModifiedBy\", \"LanguageId\", \"IsDeleted\", \"IsSend\" FROM msg.\"FeatureNotification\" n " +
                                        "WHERE n.\"IsDeleted\" = '0' {0} {1} {2}", conditions, orderBy, paging);

            setting.Connection += "Database=" + database;
            var obj = DatabaseHelper.ExcuteSql(totalCountQuery, setting).Rows;
            int totalCount = 0;

            if (obj != null && obj.Count > 0)
            {
                totalCount = int.Parse(obj[0]["count"].ToString());
            }
            var FeatureNotifications = DatabaseHelper.ExcuteQueryToList<FeatureNotificationDataModel>(sql, setting);

            return new DataResultsModel()
            {
                TotalCount = totalCount,
                Items = (IList)FeatureNotifications
            };
        }

        /// <summary>
        /// LVTan
        /// Create FeatureNotification
        /// </summary>
        /// <param name="model"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        public async Task<bool> Create(FeatureNotificationModel model, ClaimsPrincipal User, string token)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            FireBaseService fireBaseService = new FireBaseService(_options);
            bool isExists = CheckExists(-1, model.Title);

            if (isExists)
            {
                return false;
            }
            var userId = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
            string query = "INSERT INTO msg.\"FeatureNotification\"(\"Title\", \"ImageUrl\", \"ShortDescription\", \"Description\", \"IsDeleted\", \"CreateBy\", \"ModifiedBy\", \"LanguageId\") " +
                            "VALUES ('{0}', '{1}', '{2}', '{3}', '0', '{4}', '{5}', {6}) RETURNING \"Id\";";
            string sql = String.Format(query, model.Title.Replace("'", "''"), model.ImageUrl, model.ShortDescription.Replace("'", "''"), model.Description.Replace("'", "''"), userId, userId, model.LanguageId);

            setting.Connection += "Database=" + database;

            //Save record and get back inserted id
            var obj = DatabaseHelper.ExcuteSql(sql, setting).Rows;
            string insertedId = string.Empty;

            if (obj != null && obj.Count > 0)
            {
                insertedId = obj[0]["Id"].ToString();
                var data = await SendToUser(long.Parse(insertedId), model.LanguageId, token);
                if (data != null)
                {
                    //send notification to user
                    var dataSendFireBase = new SendFireBaseDataRequest
                    {
                        RecipientIds = data.Select(c => c.Key).ToList(),
                        Event = Constants.FireBase.EventFeatureCount
                    };
                    fireBaseService.SendFeatureData(dataSendFireBase, model);
                }
            }

            if (string.IsNullOrEmpty(insertedId) || Int32.Parse(insertedId) <= 0)
            {
                return false;
            }
            return long.Parse(insertedId) > 0;
        }

        /// <summary>
        /// LVTan
        /// Update FeatureNotification
        /// </summary>
        /// <param name="model"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        public bool Update(FeatureNotificationModel model, ClaimsPrincipal User)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            var userId = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
            string sql = string.Format("UPDATE msg.\"FeatureNotification\" " +
                                        "SET \"Title\" = '{0}', " +
                                            "\"ShortDescription\" = '{1}', " +
                                            "\"Description\" = '{2}', " +
                                            "\"ModifiedDate\" = (SELECT CURRENT_TIMESTAMP), " +
                                            "\"ModifiedBy\" = '{3}', " +
                                            "\"LanguageId\" = {4} " +
                                        "WHERE \"Id\" = {5};",
                                        model.Title.Replace("'", "''"), model.ShortDescription.Replace("'", "''"), model.Description.Replace("'", "''"), userId, model.LanguageId, model.Id);
            setting.Connection += "Database=" + database;
            int result = DatabaseHelper.ExcuteNonQuery(sql, setting);

            return result >= 1;
        }

        /// <summary>
        /// LVTan
        /// Delete FeatureNotification
        /// </summary>
        /// <param name="curUserId"></param>
        /// <param name="FeatureNotificationId"></param>
        /// <returns></returns>
        public bool Delete(string curUserId, long FeatureNotificationId)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };

            string sql = string.Format("UPDATE msg.\"FeatureNotification\" " +
                                        "SET \"IsDeleted\" = '1', \"ModifiedDate\" = (SELECT CURRENT_TIMESTAMP), \"ModifiedBy\" = '{0}' WHERE \"Id\" = {1}",
                                        curUserId, FeatureNotificationId);
            setting.Connection += "Database=" + database;
            int result = DatabaseHelper.ExcuteNonQuery(sql, setting);

            return result >= 1;
        }
    }
}
