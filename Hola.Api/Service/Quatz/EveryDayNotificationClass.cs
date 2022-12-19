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
        public EveryDayNotificationClass(FirebaseService firebaseService,
                                         IUserService userServices,
                                         IQuestionService questionService)
        {
            this.firebaseService = firebaseService;
            _userServices = userServices;
            _questionService = questionService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var listUser = await _userServices.GetAllAsync(x => (x.isnotification == 1 && x.IsDeleted == 0));
                var response = listUser.ToList();
                foreach (var item in response)
                {
                    // Lấy ra thông tin deviceToken 
                    string userName = item.Name;
                    var devideFirebaseToken = item.DeviceToken;
                    var totalQuestion = _questionService.CountQuestionToday(item.Id);
                    PushNotificationRequest request = new PushNotificationRequest()
                    {
                        notification = new NotificationMessageBody()
                        {
                            title = $"Hi! {userName}",
                            body = $"Ngày nay bạn đã học được {totalQuestion} từ, cố gắng lên nhé"
                        },
                    };
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
