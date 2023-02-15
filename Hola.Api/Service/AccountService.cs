using Hola.Api.Models.Questions;
using Hola.Core.Common;
using Hola.Core.Model;
using Hola.Core.Service;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
                deviceToken, userId);
            var result = await Excecute(ConnectionString, sql);
            return true;
        }

        public string GetDeviceTokenByUserId(int UserId)
        {
            var sql = string.Format("SELECT devicetoken FROM qes.accounts WHERE user_id  ={0}", UserId);
            var result = FirstOrDefault<string>(ConnectionString, sql);
            return result;
        }
        /// <summary>
        /// lấy ra chỉ tiêu của người đó trong ngày
        /// </summary>
        /// <param name="userid">Userid của người lấy lích sử</param>
        /// <param name="count_target">Chỉ tiêu mặc định ví dụ 10 từ 1 ngày</param>
        /// <returns></returns>
        public async Task<bool> CreateHistoryOneDay(int userid, int count_target)
        {
            try
            {
                string sql_count = $"SELECT COUNT(1) FROM usr.question where fk_userid = {userid} and created_on::Date = CURRENT_DATE";
                var result_count = await ExcecuteScalarAsync(ConnectionString, sql_count);
                double percent_of_day = (double.Parse(result_count.ToString()) / count_target) * 100;
                string note = string.Empty;
                if (percent_of_day < 100)
                {
                    note = "Chưa hoàn thành chỉ tiêu";
                }
                else
                {
                    note = "Hoàn thành chỉ tiêu";
                }

                string sql_count_update = $"SELECT COUNT(1) FROM history.userhistory where fk_userid = {userid} and created_on::Date = CURRENT_DATE";
                var result_count_update = await ExcecuteScalarAsync(ConnectionString, sql_count_update);
                if (result_count_update != 1)
                {
                    string sql_inserst = $"INSERT INTO history.userhistory (fk_userid, count_word, percent_day, note, created_on)" +
                     $" VALUES({userid}, {result_count}, {percent_of_day}, '{note}', '{DateTime.Now.ToString("yyyy-MM-dd")}');";
                    var result_add = await Excecute(ConnectionString, sql_inserst);
                }
                else
                {
                    string sql_update = $"UPDATE history.userhistory SET count_word={result_count}, percent_day={percent_of_day}, note='{note}'," +
                        $" created_on='{DateTime.Now.ToString("yyyy-MM-dd")}'\r\nwhere fk_userid ={userid} and created_on::Date = CURRENT_DATE";
                    var result_update = await Excecute(ConnectionString, sql_update);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}
