using Hola.Api.Models.Accounts;
using Hola.Api.Service.UserServices;
using Hola.Api.Service.V1;
using Quartz;
using System.Linq;
using System.Threading.Tasks;

namespace Hola.Api.Service.Quatz
{
    public class HistoryEveryDayJob : IJob
    {
        private readonly IUserService _userServices;
        private readonly AccountService _accountService;

        public HistoryEveryDayJob(AccountService accountService, 
            IUserService userServices)
        {
            _accountService = accountService;
            _userServices = userServices;
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
        }
    }
}
