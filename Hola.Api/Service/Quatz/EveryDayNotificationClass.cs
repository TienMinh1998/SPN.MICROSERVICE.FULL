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
        private readonly IConfiguration _Configuration;
        private readonly AccountService accountService;
        private readonly FirebaseService firebaseService;
        private readonly QuestionService qesQuestionService;
        private readonly IQuestionService _questionService;
        private readonly IUserService _userServices;
        public EveryDayNotificationClass(IConfiguration configuration, AccountService accountService,
            FirebaseService firebaseService, QuestionService qesQuestionService,
            IUserService userServices,
            IQuestionService questionService)
        {
            _Configuration = configuration;
            this.accountService = accountService;
            this.firebaseService = firebaseService;
            this.qesQuestionService = qesQuestionService;
            _userServices = userServices;
            _questionService = questionService;
        }
        public async Task CheckTaskService()
        {
            try
            {
                var listUser = await _userServices.GetAllAsync(x => (x.isnotification == 1 && x.IsDeleted == 0));
                var response = listUser.ToList();
                foreach (var item in response)
                {
                    // Lấy ra thông tin deviceToken 
                    var devideFirebaseToken = item.DeviceToken;
                    PushNotificationRequest request = new PushNotificationRequest()
                    {
                        notification = new NotificationMessageBody()
                        {
                            title = "Test JobEveryDay",
                            body = "Chào ngày mới, đừng quên nhiệm vụ hôm nay nhé bạn"
                        },
                    };
                    request.registration_ids.Add(devideFirebaseToken);
                    await firebaseService.Push(request, item.Id);
                }


            }
            catch (System.Exception ex)
            {

                throw;
            }

        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await Task.WhenAll(CheckTaskService());
            }
            catch
            {
                return;
            }
        }
    }
}
