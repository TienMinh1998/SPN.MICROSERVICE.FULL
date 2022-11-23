using Hola.Api.Models;
using Hola.Core.Common;
using Hola.Core.Helper;
using Hola.Core.Model;
using Hola.Core.Model.CommonModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Linq;
using System.Security.Claims;

namespace Hola.Api.Service
{
    public class AnnouncementCategoryService
    {
        private readonly IOptions<SettingModel> _options;
        private readonly string database = Constant.MESSAGE_DB;

        public AnnouncementCategoryService(IOptions<SettingModel> setting)
        {
            _options = setting;
        }

        /// <summary>
        /// LVTan
        /// Check Exists Announcement Category
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool CheckExists(string title)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };

            string sql = String.Format("SELECT COUNT(*) FROM msg.\"AnnouncementCategory\" WHERE Trim(\"Title\") = Trim('{0}')", title);
            setting.Connection += "Database=" + database;
            int result = DatabaseHelper.ExcuteQueryToList<int>(sql, setting).FirstOrDefault();
            return result > 0;
        }

        /// <summary>
        /// LVTan
        /// Search Announcement Category
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public DataResultsModel Search(AnnouncementCategoryFilterModel filters)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };

            string paging = string.Empty;


            var take = !filters.PageSize.HasValue || filters.PageSize.Value <= 0 ? "" : $"FETCH NEXT " + filters.PageSize.Value + " ROWS ONLY";
            paging += " OFFSET " + ((filters.PageNumber ?? 1) - 1) * (filters.PageSize ?? 0) + " ROWS " + take;

            string totalCountQuery = string.Format("SELECT COUNT(1) FROM msg.\"AnnouncementCategory\" n WHERE " +
                                                   "n.\"Title\" LIKE '%" + filters.SearchText + "%' " +
                                                   "OR CAST(n.\"CreateDate\" AS VARCHAR(10)) LIKE  '%" + filters.SearchText + "%%' " +
                                                   "OR n.\"CreateBy\" LIKE '%" + filters.SearchText + "%' " +
                                                   "OR n.\"ModifiedBy\" LIKE '%" + filters.SearchText + "%' " +
                                                   "OR CAST(n.\"ModifiedDate\" AS VARCHAR(10)) LIKE '%" + filters.SearchText + "%' ");

            string sql = string.Format("SELECT \"AnnouncementCategoryId\", n.\"Title\", n.\"CreateDate\", n.\"CreateBy\", n.\"ModifiedDate\", n.\"ModifiedBy\" " +
                                       "FROM msg.\"AnnouncementCategory\" n " +
                                       "WHERE n.\"Title\" LIKE '%" + filters.SearchText + "%' " +
                                       "OR CAST(n.\"CreateDate\" AS VARCHAR(10)) LIKE  '%" + filters.SearchText + "%' " +
                                       "OR n.\"CreateBy\" LIKE '%" + filters.SearchText + "%' " +
                                       "OR n.\"ModifiedBy\" LIKE '%" + filters.SearchText + "%' " +
                                       "OR CAST(n.\"ModifiedDate\" AS VARCHAR(10)) LIKE '%" + filters.SearchText + "%' " + 
                                       "ORDER BY \"AnnouncementCategoryId\" DESC {0}", paging);

            setting.Connection += "Database=" + database;
            var obj = DatabaseHelper.ExcuteSql(totalCountQuery, setting).Rows;
            int totalCount = 0;

            if (obj != null && obj.Count > 0)
            {
                totalCount = int.Parse(obj[0]["count"].ToString());
            }

            var AnnouncementCategorys = DatabaseHelper.ExcuteQueryToList<AnnouncementCategoryModel>(sql, setting);

            return new DataResultsModel()
            {
                TotalCount = totalCount,
                Items = (IList)AnnouncementCategorys
            };
        }

        /// <summary>
        /// LVTan
        /// Create Announcement Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool Create(AnnouncementCategoryModel category, ClaimsPrincipal User)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            string userid = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
            bool isExists = CheckExists(category.Title);

            if (isExists)
            {
                return false;
            }

            string sql = string.Format("INSERT INTO msg.\"AnnouncementCategory\" (\"Title\", \"CreateDate\", \"CreateBy\", \"ModifiedDate\", \"ModifiedBy\") " +
                                        "VALUES ('{0}', '{1}', '{2}', '{3}', null) RETURNING \"AnnouncementCategoryId\";",
                                        category.Title.Replace("'", "''"), DateTime.UtcNow,userid, DateTime.UtcNow);

            setting.Connection += "Database=" + database;

            //Save record and get back inserted id
            var obj = DatabaseHelper.ExcuteSql(sql, setting).Rows;
            string insertedId = string.Empty;

            if (obj != null && obj.Count > 0)
            {
                insertedId = obj[0]["AnnouncementCategoryId"].ToString();
            }

            if (string.IsNullOrEmpty(insertedId) || Int32.Parse(insertedId) <= 0)
            {
                return false;
            }

            return Int32.Parse(insertedId) > 0;
        }

        /// <summary>
        /// LVTan
        /// Update Announcement Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool Update(AnnouncementCategoryModel category)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };

            string sql = string.Format("UPDATE msg.\"AnnouncementCategory\" " +
                                        "SET \"Title\" = '{0}', \"ModifiedDate\" = (SELECT CURRENT_TIMESTAMP), \"ModifiedBy\" = '{1}' " +
                                        "WHERE \"AnnouncementCategoryId\" = {2};", category.Title.Replace("'", "''"), category.ModifiedBy, category.AnnouncementCategoryId);
            setting.Connection += "Database=" + database;
            int result = DatabaseHelper.ExcuteNonQuery(sql, setting);

            return result >= 1;
        }

        /// <summary>
        /// LVTan
        /// Delete Announcement Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public bool Delete(long categoryId)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };

            string sql = string.Format("DELETE FROM msg.\"AnnouncementCategory\" " +
                                        "WHERE \"AnnouncementCategoryId\" = {0} ", categoryId);
            setting.Connection += "Database=" + database;
           return DatabaseHelper.ExcuteNonQuery(sql, setting) > 0;
        }
    }
}
