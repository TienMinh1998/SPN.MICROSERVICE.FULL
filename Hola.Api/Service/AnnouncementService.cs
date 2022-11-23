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
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hola.Api.Service
{
    public class AnnouncementService : BaseService
    {
        private readonly IOptions<SettingModel> _options;
        private readonly string database = Constant.MESSAGE_DB;

        public AnnouncementService(IOptions<SettingModel> setting) : base(setting)
        {
            _options = setting;
        }

        /// <summary>
        /// John
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int AnnouncemnetCount(string uid)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            string totalCountQuery =
                string.Format(
                    "SELECT COUNT(*) FROM msg.\"AnnouncementUser\" WHERE \"UserId\" = {0} AND \"IsRead\" = CAST(0 AS BIT)",
                    uid);

            var obj = DatabaseHelper.ExcuteSql(totalCountQuery, setting).Rows;
            int totalCount = 0;
            if (obj != null && obj.Count > 0)
            {
                totalCount = int.Parse(obj[0]["count"].ToString());
            }

            return totalCount;
        }

        /// <summary>
        /// LVTan
        /// Search Announcement
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public DataResultsModel Search(AnnouncementFilterModel filters)
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
                conditions += string.Format(" AND ( cate.\"Title\" ILIKE '%" + filters.SearchText + "%' " +
                                            "OR   CAST(n.\"AnnouncementId\" AS VARCHAR(10)) ILIKE '%" + filters.SearchText + "%' " +
                                            "OR   CAST(n.\"CreateDate\" AS VARCHAR(10)) ILIKE '%" + filters.SearchText + "%' " +
                                            "OR   CAST(n.\"ModifiedDate\" AS VARCHAR(10)) ILIKE '%" + filters.SearchText + "%' " +
                                            "OR   n.\"CreateBy\" ILIKE '%" + filters.SearchText + "%' " +
                                            "OR   n.\"ModifiedBy\" ILIKE '%" + filters.SearchText + "%' " +
                                            "OR   n.\"Html\" ILIKE '%" + filters.SearchText + "%' )" +
                                            "AND n.\"IsDeleted\" = '0'", filters.SearchText);
            }
            else
            {
                conditions += " AND n.\"IsDeleted\" = '0'";
            }
            if (filters.LanguageId.HasValue)
            {
                conditions += string.Format(" AND n.\"LanguageId\" = {0}", filters.LanguageId.Value);
            }
            if (filters.CountryId.HasValue)
            {
                conditions += string.Format(" AND n.\"CountryId\" = {0}", filters.CountryId.Value);
            }

            var take = !filters.PageSize.HasValue || filters.PageSize.Value <= 0 ? "" : $"FETCH NEXT " + filters.PageSize.Value + " ROWS ONLY";
            paging += " OFFSET " + ((filters.PageNumber ?? 1) - 1) * (filters.PageSize ?? 0) + " ROWS " + take;

            string totalCountQuery = string.Format("SELECT COUNT(1) FROM msg.\"Announcement\" n LEFT JOIN msg.\"AnnouncementCategory\" cate ON n.\"CategoryId\" = cate.\"AnnouncementCategoryId\" WHERE 1 = 1 {0}", conditions);

            string sql = string.Format("SELECT n.\"AnnouncementId\", n.\"LanguageId\", n.\"CountryId\", n.\"CategoryId\",cate.\"Title\", n.\"Html\", n.\"IsDeleted\", " +
                                        "n.\"CreateDate\", n.\"CreateBy\", n.\"ModifiedDate\", n.\"ModifiedBy\", n.\"IsMainAnnouncement\",n.\"IsPosted\" " +
                                        "FROM msg.\"Announcement\" n " +
                                        "LEFT JOIN msg.\"AnnouncementCategory\" cate ON n.\"CategoryId\" = cate.\"AnnouncementCategoryId\" " +
                                        "WHERE 1 = 1 {0} ORDER BY n.\"IsMainAnnouncement\" DESC, n.\"AnnouncementId\" DESC {1}", conditions, paging);

            setting.Connection += "Database=" + database;
            var obj = DatabaseHelper.ExcuteSql(totalCountQuery, setting).Rows;
            int totalCount = 0;

            if (obj != null && obj.Count > 0)
            {
                totalCount = int.Parse(obj[0]["count"].ToString());
            }
            var Announcements = DatabaseHelper.ExcuteQueryToList<AnnouncementDataModel>(sql, setting);

            return new DataResultsModel()
            {
                TotalCount = totalCount,
                Items = (IList)Announcements
            };
        }

        /// <summary>
        /// John
        /// </summary>
        /// <param name="announcementId"></param>
        /// <returns></returns>
        public AnnouncementResponse Get(long announcementId)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            string sqlCommand = string.Format(
                "SELECT a.\"AnnouncementId\", a.\"Html\", a.\"LanguageId\", a.\"CategoryId\", " +
                "a.\"CreateDate\" AS \"CreatedDate\", a.\"ModifiedDate\" AS \"ModifiedDate\"," +
                "a.\"CreateBy\", a.\"ModifiedBy\", c.\"Title\" AS \"Category\", a.\"LanguageId\", a.\"CountryId\", a.\"IsMainAnnouncement\", a.\"IsPosted\" " +
                "FROM msg.\"Announcement\" a " +
                "LEFT JOIN msg.\"AnnouncementCategory\" c ON a.\"CategoryId\" = c.\"AnnouncementCategoryId\" " +
                "WHERE a.\"AnnouncementId\" = {0} AND a.\"IsDeleted\" = CAST(0 AS BIT)", announcementId);

            return DatabaseHelper.ExcuteQueryToList<AnnouncementResponse>(sqlCommand, setting).FirstOrDefault();
        }

        /// <summary>
        /// LVTan
        /// Create Announcement
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<AnnouncementResponse> Create(AnnouncementModel model)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };

            string sql = string.Format("INSERT INTO msg.\"Announcement\"(\"LanguageId\", \"CountryId\", \"CategoryId\", \"Html\", \"IsDeleted\", \"CreateBy\",\"CreateDate\") " +
                                        "VALUES ({0}, {1}, {2}, '{3}', '0', '{4}','{5}') RETURNING \"AnnouncementId\";",
                                        model.LanguageId, model.CountryId, model.CategoryId, model.Html.Replace("'", "''"), model.CreateBy, DateTime.UtcNow);

            setting.Connection += "Database=" + database;

            //Save record and get back inserted id
            var obj = DatabaseHelper.ExcuteSql(sql, setting).Rows;
            long insertedId = 0;

            if (obj != null && obj.Count > 0)
            {
                insertedId = Int64.Parse(obj[0]["AnnouncementId"].ToString());
            }
            var announcement = Get(insertedId);
            return announcement;
        }

        public async Task<AnnouncementResponse> PostToUser(AnnouncementModel model, string token)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            APICrossHelper aPICrossHelper = new APICrossHelper();
            var result = await aPICrossHelper.Get<JsonResponseModel>(_options.Value.UserServiceUrl + "/User/GetUserId?countryId=" + model.CountryId + "&languageId="+model.LanguageId, token);
            if (result.Data != null)
            {
                model.UserIdsInCountry = JsonConvert.DeserializeObject<List<string>>(result.Data.ToString());
            }
            if (model.AnnouncementId <= 0)
            {
                return null;
            }

            List<KeyValuePair<string, long>> announcementList = new List<KeyValuePair<string, long>>();

            foreach (var item in model.UserIdsInCountry)
            {
                string announcementUserQuery = string.Format("INSERT INTO msg.\"AnnouncementUser\"(\"AnnouncementId\", \"UserId\", \"IsRead\") VALUES ({0}, '{1}', '0');", model.AnnouncementId, item);
                DatabaseHelper.ExcuteNonQuery(announcementUserQuery, setting);
                KeyValuePair<string, long> keyValueItem = new KeyValuePair<string, long>(item, model.AnnouncementId);
                announcementList.Add(keyValueItem);
            }
            string sql = string.Format("UPDATE msg.\"Announcement\" SET \"IsPosted\" = CAST(1 AS BIT) WHERE \"AnnouncementId\" = {0}", model.AnnouncementId);
            DatabaseHelper.ExcuteNonQuery(sql, setting);
            var announcement = Get(model.AnnouncementId);
            announcement.UserAnnouncementMap = announcementList;
            return announcement;
        }

        public List<string> UnPostToUser(long announcementId)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            //List<KeyValuePair<string, long>> announcementList = new List<KeyValuePair<string, long>>();
            string announcementUserQuery = string.Format("SELECT \"UserId\" FROM msg.\"AnnouncementUser\" " +
                "WHERE \"AnnouncementId\" = {0}", announcementId);
            var data1 = DatabaseHelper.ExcuteSql(announcementUserQuery, setting);
            var useridList = data1.Rows.OfType<DataRow>().Select(dr => dr.Field<string>("UserId")).ToList();

            //foreach (var item in useridList)
            //{
            //    KeyValuePair<string, long> keyValueItem = new KeyValuePair<string, long>(item, announcementId);
            //    announcementList.Add(keyValueItem);
            //}

            string sql = string.Format("DELETE FROM msg.\"AnnouncementUser\" WHERE \"AnnouncementId\" = {0};", announcementId);
            DatabaseHelper.ExcuteNonQuery(sql, setting);

            string sql1 = string.Format("UPDATE msg.\"Announcement\" SET \"IsPosted\" = CAST(0 AS BIT) WHERE \"AnnouncementId\" = {0}", announcementId);
            DatabaseHelper.ExcuteNonQuery(sql1, setting);
            return useridList;
        }

        /// <summary>
        /// LVTan
        /// Update Announcement
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(AnnouncementModel model)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };

            string sql = string.Format("UPDATE msg.\"Announcement\" " +
                                        "SET \"LanguageId\"={0}, " +
                                            "\"CountryId\"={1}, " +
                                            "\"CategoryId\"={2}, " +
                                            "\"Html\"='{3}', " +
                                            "\"ModifiedDate\"=(SELECT CURRENT_TIMESTAMP), " +
                                            "\"ModifiedBy\"='{4}' " +
                                            "WHERE \"AnnouncementId\" = {5};", model.LanguageId, model.CountryId, model.CategoryId,
                                            model.Html.Replace("'", "''"), model.ModifiedBy, model.AnnouncementId);
            setting.Connection += "Database=" + database;
            int result = DatabaseHelper.ExcuteNonQuery(sql, setting);

            return result >= 1;
        }

        public bool UpdateIsMainAnnouncement(bool model, long announcementId)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            string setAll = string.Format("UPDATE msg.\"Announcement\" " +
                                        "SET \"IsMainAnnouncement\"=Cast(0 as bit)");
            DatabaseHelper.ExcuteNonQuery(setAll, setting);
            string sql = string.Format("UPDATE msg.\"Announcement\" " +
                                        "SET \"IsMainAnnouncement\"=Cast({0} as bit) " +
                                            "WHERE \"AnnouncementId\" = {1};", Convert.ToInt32(model), announcementId);

            int result = DatabaseHelper.ExcuteNonQuery(sql, setting);

            return result >= 1;
        }

        /// <summary>
        /// LVTan
        /// Delete Announcement
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(string userId, long id)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };

            string sql = string.Format("UPDATE msg.\"Announcement\" " +
                                        "SET \"IsDeleted\" = '1', \"ModifiedDate\" = (SELECT CURRENT_TIMESTAMP), \"ModifiedBy\" = '{0}' " +
                                        "WHERE \"AnnouncementId\" = {1}", userId, id);
            setting.Connection += "Database=" + database;
            int result = DatabaseHelper.ExcuteNonQuery(sql, setting);

            return result >= 1;
        }

        /// <summary>
        /// LVTan
        /// Set As Read
        /// </summary>
        /// <param name="curUserId"></param>
        /// <param name="announcementIds"></param>
        /// <returns></returns>
        public bool SetAsRead(List<long> announcementIds, ClaimsPrincipal User)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            string userId = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
            string sql = string.Format("UPDATE msg.\"AnnouncementUser\" " +
                                        "SET \"IsRead\" = '1' " +
                                        "WHERE \"UserId\" = '{0}' AND \"AnnouncementId\" IN ({1})", userId, string.Join(", ", announcementIds));
            setting.Connection += "Database=" + database;
            int result = DatabaseHelper.ExcuteNonQuery(sql, setting);

            return result >= 1;
        }

        public bool SetAsReadAll(ClaimsPrincipal User)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            string userId = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
            string sql = string.Format("UPDATE msg.\"AnnouncementUser\" " +
                                       "SET \"IsRead\" = '1' " +
                                       "WHERE \"UserId\" = '{0}' ", userId);
            setting.Connection += "Database=" + database;
            int result = DatabaseHelper.ExcuteNonQuery(sql, setting);

            return result >= 1;
        }

        /// <summary>
        /// John
        /// </summary>
        /// <param name="request"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        public DataResultsModel GetUserAnnouncement(SearchAnnouncementRequest request, ClaimsPrincipal User)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            string orderBy = " ORDER BY a.\"IsMainAnnouncement\" DESC, a.\"CreateDate\" DESC" + (string.IsNullOrEmpty(request.OrderDirection) ? string.Empty : request.OrderDirection);
            string paging = string.Empty;
            var take = !request.PageSize.HasValue || request.PageSize.Value <= 0 ? "" : $"FETCH NEXT " + request.PageSize.Value + " ROWS ONLY";
            paging += " OFFSET " + ((request.PageNumber ?? 1) - 1) * (request.PageSize ?? 0) + " ROWS " + take;
            string userId = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
            var sqlCount = string.Format("SELECT COUNT(1) " +
                                         "FROM msg.\"AnnouncementUser\" u " +
                                         "LEFT JOIN msg.\"Announcement\" a ON u.\"AnnouncementId\" = a.\"AnnouncementId\" " +
                                         "WHERE a.\"IsDeleted\" = CAST(0 AS BIT) AND u.\"UserId\" = '{0}';", userId);

            var sqlUnReadCount = string.Format("SELECT COUNT(1) " +
                                         "FROM msg.\"AnnouncementUser\" u " +
                                         "LEFT JOIN msg.\"Announcement\" a ON u.\"AnnouncementId\" = a.\"AnnouncementId\" " +
                                         "WHERE a.\"IsDeleted\" = CAST(0 AS BIT) AND u.\"IsRead\" = CAST(0 AS BIT) AND u.\"UserId\" = '{0}';", userId);

            var sqlCommand = string.Format(
                "SELECT a.\"AnnouncementId\", a.\"Html\", a.\"CreateDate\" AS \"Date\",u.\"IsRead\" AS \"Read\" " +
                "FROM msg.\"AnnouncementUser\" u " +
                "LEFT JOIN msg.\"Announcement\" a ON u.\"AnnouncementId\" = a.\"AnnouncementId\" " +
                "WHERE a.\"IsDeleted\" = CAST(0 AS BIT) AND u.\"UserId\" = '{0}' {1} {2}", userId, orderBy, paging);
            var obj = DatabaseHelper.ExcuteSql(sqlCount, setting).Rows;

            int totalCount = 0;
            if (obj != null && obj.Count > 0)
            {
                var objUnread = DatabaseHelper.ExcuteSql(sqlUnReadCount, setting).Rows;
                int unreadCount = 0;
                if (objUnread != null && objUnread.Count > 0)
                {
                    unreadCount = int.Parse(objUnread[0]["count"].ToString());
                }

                totalCount = int.Parse(obj[0]["count"].ToString());
                if (totalCount != 0)
                {
                    if (totalCount > 1000)
                    {
                        try
                        {
                            string sqlList = string.Format("SELECT \"AnnouncementId\" FROM msg.\"Announcement\" ORDER BY \"CreateDate\" LIMIT {0};", totalCount - 1000);
                            var data1 = DatabaseHelper.ExcuteSql(sqlList, setting);
                            var AnnouncementIdList = data1.Rows.OfType<DataRow>().Select(dr => dr.Field<long>("AnnouncementId")).ToList();
                            string deleteSql = "DELETE  FROM msg.\"AnnouncementUser\" WHERE \"AnnouncementId\" IN (" + String.Join(",", Array.ConvertAll(AnnouncementIdList.ToArray(), z => "'" + z + "'")) + ")";
                            DatabaseHelper.ExcuteNonQuery(deleteSql, setting);
                            string deleteAnnouncement = "DELETE  FROM msg.\"Announcement\" WHERE \"AnnouncementId\" IN (" + String.Join(",", Array.ConvertAll(AnnouncementIdList.ToArray(), z => "'" + z + "'")) + ")";
                            DatabaseHelper.ExcuteNonQuery(deleteAnnouncement, setting);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }

                    //var announcement = DatabaseHelper.ExcuteQueryToList<AnnouncementClientModel>(sqlCommand, setting);
                    var announcement = QueryToList<AnnouncementClientModel>(rawConnection + database, sqlCommand);
                    return new DataResultsModel()
                    {
                        TotalCount = totalCount,
                        Items = (IList)announcement,
                        UnreadCount = unreadCount
                    };
                }
            }
            return null;
        }

        /// <summary>
        /// John
        /// </summary>
        /// <param name="request"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        public int GetUnReadCount(ClaimsPrincipal User)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            string userId = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;

            var sqlUnReadCount = string.Format("SELECT COUNT(1) " +
                                         "FROM msg.\"AnnouncementUser\" u " +
                                         "LEFT JOIN msg.\"Announcement\" a ON u.\"AnnouncementId\" = a.\"AnnouncementId\" " +
                                         "WHERE a.\"IsDeleted\" = CAST(0 AS BIT) AND u.\"IsRead\" = CAST(0 AS BIT) AND u.\"UserId\" = '{0}';", userId);

            var objUnread = DatabaseHelper.ExcuteSql(sqlUnReadCount, setting).Rows;
            int unreadCount = 0;
            if (objUnread != null && objUnread.Count > 0)
            {
                unreadCount = int.Parse(objUnread[0]["count"].ToString());
            }
            return unreadCount;
        }
    }
}