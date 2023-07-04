using Hola.Api.Models;
using Hola.Api.Models.Accounts;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.UserServices;
using Hola.Api.Service.V1;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hola.Api.Service.Quatz
{
    public class HistoryEveryDayJob : IJob
    {
        private readonly IUserService _userServices;
        private readonly AccountService _accountService;
        private readonly DapperBaseService _dapper;
        public HistoryEveryDayJob(AccountService accountService,
            IUserService userServices,
            DapperBaseService dapper)
        {
            _accountService = accountService;
            _userServices = userServices;
            _dapper = dapper;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var listUser = await _userServices.GetAllAsync();
            var response = listUser.ToList();
            foreach (var item in response)
            {
                int userid = item.Id;
                int targetOFDay = 10;
                await _accountService.CreateHistoryOneDay(userid, targetOFDay);
            }

            // Thống kê tổng số từ hôm nay
            var dateTimeNow = DateTime.UtcNow.ToString("yyyy/dd/MM");
            string query = $"\r\nSELECT\r\n (SELECT COUNT(1) FROM \"public\".\"QuestionStandards\" WHERE created_on >= '{dateTimeNow}') AS TotalWord,\r\n    (SELECT COUNT(1) FROM \"usr\".\"Reading\" r WHERE \"CreatedDate\" >= '{dateTimeNow}') AS TotalPost;";
            var count = _dapper.QueryFirstOrDefault<OverviewResult>(query);
            if (count != null)
            {
                int countToday = _dapper.QueryFirstOrDefault<int>($"SELECT count(1) FROM usr.report where created_on >= '{dateTimeNow}'");
                if (countToday == 0)
                {
                    string queryInserst = $"INSERT INTO usr.report\r\n(\"FK_UserId\", \"TotalWords\", \"TotalPosts\", created_on)\r\nVALUES(1, {count.totalword}, {count.totalpost}, '{dateTimeNow}');";
                    await _dapper.Execute(query);
                }
                else
                {
                    string queryInserst = $"UPDATE usr.report\r\nSET \"FK_UserId\"=1, \"TotalWords\"={count.totalword}, \"TotalPosts\"={count.totalpost}, created_on='{dateTimeNow}'\r\nWHERE \"created_on\" >= '{dateTimeNow}';";
                    await _dapper.Execute(query);
                }

            }

        }
    }
}
