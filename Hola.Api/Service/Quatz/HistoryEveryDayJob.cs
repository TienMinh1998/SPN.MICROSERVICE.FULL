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
            string query = $"\r\nSELECT\r\n (SELECT COUNT(1) FROM \"public\".\"QuestionStandards\" WHERE created_on >= '{dateTimeNow}') AS TotalWord,\r\n    (SELECT COUNT(1) FROM \"usr\".\"Reading\" r WHERE \"CreatedDate\" >= '{dateTimeNow}') AS TotalPost;";
            var count = _dapper.QueryFirstOrDefault<OverviewResult>(query);

            var today = DateTime.UtcNow.Date;

            if (count != null)
            {
                var report = await _reportService.GetFirstOrDefaultAsync(x => x.created_on >= today);
                if (report == null)
                {
                    Report reportEntity = new()
                    {
                        created_on = DateTime.UtcNow,
                        FK_UserId = 1,
                        TotalPosts = count.totalpost,
                        TotalWords = count.totalword
                    };
                    var res = await _reportService.AddAsync(reportEntity);
                }
                else
                {
                    report.TotalWords = count.totalword;
                    report.TotalPosts = count.totalpost;
                    report.created_on = DateTime.UtcNow;
                    var res = _reportService.UpdateAsync(report);
                }

            }

        }
    }
}
