using Hola.Api.Models;
using Hola.Core.Common;
using Hola.Core.Helper;
using Hola.Core.Model;
using Hola.Core.Model.CommonModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;
using Hola.Api.Common;
using System.Linq;
using System.Security.Claims;
using Hola.Core.Service;
using System;

namespace Hola.Api.Service
{
    public class AgentApplicationService : BaseService
    {
        private readonly IOptions<SettingModel> _options;
        private readonly string database = Constant.ACCCOUNT_DB;

        public AgentApplicationService(IOptions<SettingModel> options) : base(options)
        {
            _options = options;
        }


        /// <summary>
        /// NVMTien
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public DataResultsModel Search(AgentApplicationSearchRequest filters)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + database;
            string orderBy = string.Empty;
            string paging = string.Empty;
            int totalCount = 0;
            string filter = string.Empty;
            if (!string.IsNullOrEmpty(filters.OrderBy))
            {
                orderBy += $",\"{filters.OrderBy}\"" + " " +
                    (string.IsNullOrEmpty(filters.OrderDirection) ? string.Empty : filters.OrderDirection);
            }
            else
            {
                orderBy += ", ap.\"AppliedDate\" DESC ";
            }

            var take = !filters.PageSize.HasValue || filters.PageSize.Value <= 0 ? "" : $"FETCH NEXT " + filters.PageSize.Value + " ROWS ONLY";
            paging += " OFFSET " + ((filters.PageNumber ?? 1) - 1) * (filters.PageSize ?? 0) + " ROWS " + take;

            if (filters.CountryId.HasValue)
            {
                filter += string.Format(" AND u.\"CountryId\" = {0}", filters.CountryId.Value);
            }
            if (filters.StatusId.HasValue)
            {
                filter += string.Format(" AND ap.\"StatusId\" = {0}", filters.StatusId.Value);
            }


            //GetListApply
            string _countQuery = $"SELECT COUNT(1) FROM usr.\"Apply\" as ap " +
                            "INNER JOIN usr.\"Users\" as u ON ap.\"UserId\" = u.\"UserId\" " +
                            $"WHERE (u.\"UserId\" = '{filters.SearchText}' " +
                            $"OR CAST(u.\"CountryId\" AS VARCHAR(3)) LIKE '%{filters.SearchText}%' " +
                            $"OR u.\"PhoneNumber\" LIKE '%{filters.SearchText}%' " +
                            $"OR CAST(ap.\"AppliedDate\" AS VARCHAR(10)) LIKE '%{filters.SearchText}%' " +
                            $"or u.\"Username\" LIKE '%{filters.SearchText}%')  {filter} ";

            // SQL COUNT 
            string sqlcmd = $"SELECT ap.\"StatusId\", ap.\"Id\" as \"Id\" ,  u.\"UserId\", u.\"Username\" as \"User\", u.\"CountryId\" as \"CountryId\", u.\"PhoneNumber\" as \"UserPhone\", ap.\"AppliedDate\" as \"AppliedDate\" FROM usr.\"Apply\" as ap " +
                      "INNER JOIN usr.\"Users\" as u ON ap.\"UserId\" = u.\"UserId\" " +
                      $"WHERE (u.\"UserId\" LIKE '%{filters.SearchText}%' " +
                      $"OR u.\"Username\" LIKE '%{filters.SearchText}%' " +
                      $"OR CAST(u.\"CountryId\" AS VARCHAR(3)) LIKE '%{filters.SearchText}%' " +
                      $"OR u.\"PhoneNumber\" LIKE '%{filters.SearchText}%' " +
                      $"OR CAST(ap.\"AppliedDate\" AS VARCHAR(10)) LIKE '%{filters.SearchText}%') " +
                      $"{filter} " +
                      $"ORDER BY ap.\"StatusId\" ASC  {orderBy} {paging}";

            // GetAllDocuments 
            string sqldoc = "SELECT appdoc.\"DocumentId\", appdoc.\"Id\", appdoc.\"ApplyId\", appdoc.\"Name\", appdoc.\"Ext\", appdoc.\"FileUrl\" From usr.\"ApplyDocument\" as appdoc ";


            
            var documents = QueryToList<AgentApplyDocumentResponse>(rawConnection + Constant.ACCCOUNT_DB, sqldoc);
            var applys = QueryToList<AgentApplicationResponse>(rawConnection + Constant.ACCCOUNT_DB, sqlcmd);
            var _count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(_countQuery, setting));

            foreach (var item in applys)
            {
                var list_doc = documents.Where(x => x.ApplyId.Equals(item.Id)).ToList();
                item.Documents = list_doc;
            }
            return new DataResultsModel(applys, _count);
        }

        // Author : NVMTien
        // CreateDate :  4-4-2022
        // Edit : not Yet 
        // Content : Return History of AgencyManagement, Have Pading
        public DataResultsModel History(string userID, int? currentPage, int? pageLimit)
        {
           
            string sqlCommand = string.Format("SELECT AC.\"StatusBeforeId\",AC.\"StatusAfterId\", AC.\"Reason\" ," +
                "AC.\"ChangeDate\" as \"ChangeDate\" FROM " +
                "usr.\"ApplyStatusChangeHistory\" AS AC INNER JOIN usr.\"Apply\"  AS ap " +
                "ON AC.\"ApplyId\" = ap.\"Id\" WHERE ap.\"UserId\" ='{0}'", userID); // Câu lệnh dữ liệu thuần

            sqlCommand = Util.ToPading(sqlCommand, currentPage, pageLimit);         // Phân trang
            
            //List<ApplyStatusChangeResponse> historyList = DatabaseHelper.ExcuteQueryToList<ApplyStatusChangeResponse>(sqlCommand, setting);
            List<ApplyStatusChangeResponse> historyList = QueryToList<ApplyStatusChangeResponse>(rawConnection + database, sqlCommand);

            return DataResultsModel.ReturnValues(historyList);
        }

        public ReviewAgentApplicationResponse ReviewAgentApplication(ReviewAgentApplicationRequest request, string user)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection + "Database=" + database,
                Provider = _options.Value.Provider
            };

            var response = new ReviewAgentApplicationResponse();
            string sql_text = $"SELECT * FROM usr.ReviewAgentApplication({request.Id},{request.StatusId},'{user}','{(request.Reason != null ? request.Reason.Replace("'", "''") : string.Empty)}');";
            int res_SQl = (int)DatabaseHelper.ExecuteScalar(sql_text, setting);
            response.Result = res_SQl == 1;
            if (res_SQl == -1) response.ErrorMessage = "already.store.member";
            List<string> listuser = new List<string>();
            //   UpdateStatus(listuser);
            return response;
        }

        public async void UpdateStatus(List<string> recipientIds, short statusAfterId)
        {
            PingService.ApplyAgentStatusChanged(new List<string>
            {
                recipientIds.ToString()
            });
            FireBaseService fireBaseService = new FireBaseService(_options);
            APICrossHelper aPICrossHelper = new APICrossHelper();
            var eventApply = string.Empty;
            if(statusAfterId == 1)
            {
                eventApply = Constants.FireBase.EventApplyAgentStatusApproved;
            }
            else if(statusAfterId == 2)
            {
                eventApply = Constants.FireBase.EventApplyAgentStatusDeclined;
            }
            else if(statusAfterId == 0)
            {
                eventApply = Constants.FireBase.EventApplyAgentStatusUnderReview;
            }
            var dataSendFireBase = new SendFireBaseDataRequest
            {
                RecipientIds = recipientIds,
                Event = eventApply
            };
            await fireBaseService.SendApplyAgentStatus(dataSendFireBase);
        }

        public class ReviewAgentApplicationResponse
        {
            public bool Result { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}