using Hola.Core.Common;
using Hola.Core.Model;
using Hola.Core.Service;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Hola.Api.Service
{
    public class AccountService : BaseService
    {
        private readonly IOptions<SettingModel> _options;
        private readonly string database = Constant.DEFAULT_DB;
        private string ConnectionString = string.Empty;

        public AccountService(IOptions<SettingModel> options) : base(options)
        {
            _options = options;
            ConnectionString = _options.Value.Connection + "Database=" + database;
        }

        /// <summary>
        /// Update DeviceToken to server know Who is Using Device
        /// </summary>
        /// <param name="deviceToken"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> UpdateDeviceTokenFirebaseAsync(string deviceToken, int userId)
        {
            string sql = string.Format("UPDATE qes.accounts SET devicetoken = '{0}' WHERE user_id = {1}",
                deviceToken,userId);
            var result = await Excecute(ConnectionString, sql);
            return true;
        }

    }
}
