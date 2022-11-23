using Hola.Api.Models;
using Hola.Core.Common;
using Hola.Core.Helper;
using Hola.Core.Model;
using Hola.Core.Model.CommonModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using FirebaseAdmin.Messaging;

namespace Hola.Api.Service
{
    public class ApplyAgentService
    {
        private readonly IOptions<SettingModel> _options;
        private readonly string database = Constant.ACCCOUNT_DB;
        private readonly Hola.Core.Helper.Extensions _extensions = new Hola.Core.Helper.Extensions();
        private readonly IHostingEnvironment hostingEnvironment;

        public ApplyAgentService(IOptions<SettingModel> setting)
        {
            _options = setting;
        }

        public AgentApplicationResponse Get(long id)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            var sql = string.Format("SELECT a.\"Id\",a.\"StatusId\",a.\"UserId\",a.\"AppliedDate\", " +
                                    "a.\"StatusId\" AS \"Status\", u.\"CountryId\", u.\"PhoneNumber\" as \"UserPhone\" " +
                                    "FROM usr.\"Apply\" AS a  " +
                                    "INNER JOIN usr.\"Users\" AS u ON u.\"UserId\" = a.\"UserId\" " +
                                    "WHERE a.\"Id\" = {0};", id);
            var result = DatabaseHelper.ExcuteQueryToList<AgentApplicationResponse>(sql, setting).FirstOrDefault();
            var docs = SearchApplicationDocuments(result.Id);
            var map = docs.GroupBy(c => c.Id).ToDictionary(k => k.Key, v => v.ToList());
            result.Documents = map.ContainsKey(result.Id) ? map[result.Id] : new List<AgentApplyDocumentResponse>();
            result.UserPhone = _extensions.MaskPhoneNumber(result.UserPhone,2);
            return result;
        }


        public List<AgentApplyDocumentResponse> SearchApplicationDocuments(long applyId)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            
            var sql = string.Format ("SELECT \"Id\", \"ApplyId\", \"DocumentId\", \"Name\", \"Ext\", \"Size\", \"FileUrl\", \"UploadedDate\" " +
                      "FROM usr.\"ApplyDocument\" AS ad " +
                      "WHERE 1=1 AND \"ApplyId\" = {0} " +
                      "ORDER BY \"UploadedDate\"", applyId );
            var result = DatabaseHelper.ExcuteQueryToList<AgentApplyDocumentResponse>(sql, setting);

            return result;
        }

        public DataResultsModel Search(AgentApplicationFilter model)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            string orderBy = string.Empty;
            string paging = string.Empty;
            if (!string.IsNullOrEmpty(model.OrderBy))
            {
                orderBy += " ORDER BY \"" + model.OrderBy + "\" " + (string.IsNullOrEmpty(model.OrderDirection) ? string.Empty : model.OrderDirection);
            }
            var take = !model.PageSize.HasValue || model.PageSize.Value <= 0 ? "" : $"FETCH NEXT " + model.PageSize.Value + " ROWS ONLY";
            paging += " OFFSET " + ((model.PageNumber ?? 1) - 1) * (model.PageSize ?? 0) + " ROWS " + take;

            string sqlCount = string.Format("SELECT COUNT(1)" +
                                         "FROM usr.\"Apply\" a" +
                                         " WHERE 1=1 ");
            string sql = string.Format("SELECT a.\"Id\",a.\"StatusId\",a.\"UserId\", a.\"AppliedDate\"" +
                                       ",u.\"CountryId\",u.\"PhoneNumber\" as \"UserPhone\"" +
                                       "FROM usr.\"Apply\" AS a " +
                                       "INNER JOIN usr.\"Users\" AS u ON u.\"UserId\" = a.\"UserId\" " +
                                       "WHERE 1=1 {0} {1};", orderBy, paging);

            var obj = DatabaseHelper.ExcuteSql(sqlCount, setting).Rows;
            int totalCount = 0;
            if (obj != null && obj.Count > 0)
            {
                totalCount = int.Parse(obj[0]["count"].ToString());
            }
            var agentApplication = DatabaseHelper.ExcuteQueryToList<AgentApplicationResponse>(sql, setting);
            return new DataResultsModel()
            {
                TotalCount = totalCount,
                Items = (IList)agentApplication
            };
        }

        public ReviewAgentApplicationRequest GetStatusBefore(int applyId)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            string sqlCommand = string.Format("SELECT \"StatusId\",\"UserId\" " +
                                              "FROM usr.\"Apply\"" +
                                              "WHERE \"Id\" = {0} LIMIT 1;", applyId);

            return DatabaseHelper.ExcuteQueryToList<ReviewAgentApplicationRequest>(sqlCommand, setting).FirstOrDefault();
        }

        //public bool ReviewAgentApplication(ReviewAgentApplicationRequest request, string userId)
        //{
        //    SettingModel setting = new SettingModel()
        //    {
        //        Connection = _options.Value.Connection,
        //        Provider = _options.Value.Provider
        //    };
        //    setting.Connection += "Database=" + database;
        //    string sqlCommand = string.Format("UPDATE usr.\"Apply\" " +
        //                                      "SET \"StatusId\" = {0} " +
        //                                      "WHERE \"Id\" = {1};" +
        //                                      "INSERT INTO usr.\"ApplyStatusChangeHistory\" (\"ApplyId\", \"StatusBeforeId\", \"StatusAfterId\", \"ChangeDate\", \"ChangeBy\", \"Reason\") " +
        //                                      "VALUES ({2}, {3}, {4}, '{5}', '{6}', '{7}');"
        //        , request.StatusId, request.Id, request.Id, request.StatusBeforeId, request.StatusId, DateTime.UtcNow,
        //        userId, request.Reason);
        //    return DatabaseHelper.ExcuteNonQuery(sqlCommand, setting) > 0;
        //}

        public List<ApplyStatusChangeResponse> ApplayAgentStatusHistory(ClaimsPrincipal User)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            var userId = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
            var sql = string.Format("SELECT a.\"StatusId\", h.\"StatusBeforeId\",h.\"StatusAfterId\",h.\"Reason\",h.\"ChangeDate\" " +
                                    "FROM usr.\"ApplyStatusChangeHistory\" h " +
                                   "INNER JOIN usr.\"Apply\" a ON a.\"Id\" = h.\"ApplyId\" AND a.\"UserId\" = '{0}' ORDER BY \"ChangeDate\"", userId);
            var applyData = DatabaseHelper.ExcuteQueryToList<ApplyStatusChangeResponse>(sql, setting);

            return applyData;
        }

        public List<ApplyStatusChangeResponse> GetApplyStatusHistoryByApply(long id)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;

            var sql = string.Format(
                "SELECT h.\"StatusBeforeId\",h.\"StatusAfterId\",h.\"Reason\",h.\"ChangeDate\"" +
                " FROM usr.\"ApplyStatusChangeHistory\" h " +
                "WHERE h.\"ApplyId\" = {0} " +
                "ORDER BY \"ChangeDate\";", id);
            return DatabaseHelper.ExcuteQueryToList<ApplyStatusChangeResponse>(sql, setting);
        }

        public int InsertStatusChangeHistory(long applyId, string userId)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            string sql = string.Empty;
            sql = string.Format(
                "INSERT INTO usr.\"ApplyStatusChangeHistory\" (\"ApplyId\", \"StatusAfterId\", \"ChangeDate\", \"ChangeBy\") " +
                "VALUES({0}, 0, '{1}', '{2}') RETURNING \"Id\";", applyId, DateTime.UtcNow, userId);
            var objSql = DatabaseHelper.ExcuteSql(sql, setting).Rows;
            int id = 0;
            if (objSql != null && objSql.Count > 0)
            {
                id = int.Parse(objSql[0]["Id"].ToString());
                if (id == 0)
                {
                    string sqlDelete = string.Format("DELETE FROM usr.\"Apply\" " +
                                                     "WHERE \"Id\" = {0}", applyId);
                    DatabaseHelper.ExcuteNonQuery(sqlDelete, setting);
                }
            }
            return id;
        }

        public int InsertApplyDocument(long applyId, long documentId, string name, string ext, int size, string fileUrl)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            string sql = string.Empty;
            sql = string.Format(
                "INSERT INTO usr.\"ApplyDocument\"(\"ApplyId\",\"DocumentId\",\"Name\",\"Ext\",\"Size\",\"FileUrl\",\"UploadedDate\") " +
                "VALUES({0},{1},'{2}','{3}',{4},'{5}','{6}') RETURNING \"Id\";", applyId, documentId, name, ext, size, fileUrl, DateTime.UtcNow);
            var objSql = DatabaseHelper.ExcuteSql(sql, setting).Rows;
            return 1;
        }

        public long ApplyForAgent(AgentApplayRequest request, ClaimsPrincipal User)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            var userId = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
            long applyId = 0;
            if (true)
            {
                string sqlcommand = $"SELECT * FROM usr.applyforagent('{userId}');";
                applyId = (int)DatabaseHelper.ExecuteScalar(sqlcommand, setting);
                if (applyId>0)
                {
                    foreach (var item in request.Documents)
                    {
                        InsertApplyDocument(applyId, item.Id, item.Name, item.Extension, item.Size,
                            item.FileUrl);
                    }
                }
            }
            return applyId;
        }
    }
}