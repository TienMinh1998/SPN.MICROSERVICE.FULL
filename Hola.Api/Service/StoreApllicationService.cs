using Hola.Api.Models;
using Hola.Core.Common;
using Hola.Core.Helper;
using Hola.Core.Model;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Hola.Api.Service
{
    public class StoreApllicationService
    {
        private readonly IOptions<SettingModel> _options;
        private readonly string databaseAccount = Constant.ACCCOUNT_DB;

        public StoreApllicationService(IOptions<SettingModel> setting)
        {
            _options = setting;
        }

        public List<ApplyStatusHistoryChange> GetApplyStatusHistoryByUser(string userId)
        {
            SettingModel setting = new SettingModel()
            {
                Connection = _options.Value.Connection,
                Provider = _options.Value.Provider
            };
            setting.Connection += "Database=" + databaseAccount;

            var sql = string.Format("SELECT a.\"StatusId\",h.\"Reason\",h.\"ChangeDate\" " +
                                    "FROM store.\"ApplyStatusChangeHistory\" h " +
                                    "LEFT JOIN usr.\"ApplyStatus\" s ON h.\"StatusBeforeId\" = s.\"Id\" " +
                                    "LEFT JOIN usr.\"ApplyStatus\" s1 ON h.\"StatusAfterId\" = s1.\"Id\" " +
                                    "INNER JOIN usr.\"Apply\" a ON a.\"Id\" = h.\"ApplyId\" AND a.\"UserId\" = '{0}' ORDER BY \"ChangeDate\"", userId);
            var applyData = DatabaseHelper.ExcuteQueryToList<ApplyStatusHistoryChange>(sql, setting);

            return applyData;
        }
    }
}