using DatabaseCore.Domain.Entities.Normals;
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
        private readonly IReportService _reportService;
        public HistoryEveryDayJob(AccountService accountService,
            IUserService userServices,
            DapperBaseService dapper,
            IReportService reportService)
        {
            _accountService = accountService;
            _userServices = userServices;
            _dapper = dapper;
            _reportService = reportService;
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
            var dateTimeNow = DateTime.UtcNow.ToString("yyyy/MM/dd");
            string query = $"select \r\nu.\"Id\" as \"UserId\",\r\n u.\"Username\" ,\r\n (SELECT COUNT(1) FROM \"public\".\"QuestionStandards\"" +
                $" WHERE created_on >= '{dateTimeNow}' and \"UserId\" = u.\"Id\")  AS TotalWord,\r\n  (SELECT COUNT(1) FROM \"usr\".\"Reading\" r " +
                $"WHERE \"CreatedDate\" >= '{dateTimeNow}' and  \"UserId\" = u.\"Id\")  AS TotalPost \r\n  from  \"usr\".\"User\" u";

            var listReport = await _dapper.GetAllAsync<OverviewResult>(query);
            if (listReport == null || listReport.Count() == 0)
            {
                return;
            }

            foreach (var item in listReport)
            {
                var today = DateTime.UtcNow.Date;
                var report = await _reportService.GetFirstOrDefaultAsync(x => x.created_on >= today && x.FK_UserId == item.UserId);
                if (report == null)
                {
                    Report reportEntity = new()
                    {
                        created_on = DateTime.UtcNow,
                        FK_UserId = item.UserId,
                        TotalPosts = item.totalpost,
                        TotalWords = item.totalword
                    };
                    var res = await _reportService.AddAsync(reportEntity);
                }
                else
                {
                    report.TotalWords = item.totalword;
                    report.TotalPosts = item.totalpost;
                    report.created_on = DateTime.UtcNow;
                    report.FK_UserId = item.UserId;
                    var res = _reportService.UpdateAsync(report);
                }
            }



        }
    }
}
