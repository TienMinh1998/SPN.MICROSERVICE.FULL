using Hola.Api.Models.Accounts;
using Hola.Api.Service.UserServices;
using Hola.Api.Service.V1;
using Microsoft.Extensions.Configuration;
using Quartz;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Hola.Api.Service.Quatz
{
    public class EveryDayNotificationClass : IJob
    {
        private readonly FirebaseService firebaseService;
        private readonly IUserService _userServices;
        private readonly IQuestionService _questionService;
        private readonly AccountService _accountService;
        public EveryDayNotificationClass(FirebaseService firebaseService,
                                         IUserService userServices,
                                         IQuestionService questionService,
                                         AccountService accountService)
        {
            this.firebaseService = firebaseService;
            _userServices = userServices;
            _questionService = questionService;
            _accountService = accountService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var listUser = await _userServices.GetAllAsync(x => (x.isnotification == 1 && x.IsDeleted == 0));
                var response = listUser.ToList();
                foreach (var item in response)
                {
                    // Get devide token infomation
                    string userName = item.Name;
                    var devideFirebaseToken = item.DeviceToken;
                    var totalQuestion = await _questionService.CountQuestionToday(item.Id);
                    PushNotificationRequest request = new PushNotificationRequest()
                    {
                        notification = new NotificationMessageBody()
                        {
                            title = $"Hi! {userName}",
                            body = $"Target today :  {totalQuestion} / 10 word"
                        }
                    };
                    await _accountService.CreateHistoryOneDay(item.Id, 10);
                    request.registration_ids.Add(devideFirebaseToken);
                    await firebaseService.Push(request, item.Id);
                }
            }
            catch
            {
                return;
            }
        }
    }
}
