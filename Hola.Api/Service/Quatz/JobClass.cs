using Hola.Api.Models.Accounts;
using Hola.Api.Service.UserServices;
using Hola.Core.Venly.Base;
using Hola.Core.Venly.WalletHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hola.Api.Service.Quatz
{
    public class JobClass : IJob
    {

        private readonly IConfiguration _Configuration;
        private readonly AccountService accountService;
        private readonly FirebaseService firebaseService;
        private readonly QuestionService qesQuestionService;
        private readonly IUserService _userServices;
        public JobClass(IConfiguration configuration, AccountService accountService, FirebaseService firebaseService, QuestionService qesQuestionService, IUserService userServices)
        {
            _Configuration = configuration;
            this.accountService = accountService;
            this.firebaseService = firebaseService;
            this.qesQuestionService = qesQuestionService;
            _userServices = userServices;
        }
        public async Task CheckTaskService()
        {
            try
            {
                // Get ListUser Noti 

                var listUser =await _userServices.GetAllAsync(x=>(x.isnotification==1 && x.IsDeleted==0));
                var response = listUser.ToList();
                foreach (var item in response)
                {
                    // Category 
                    var result = await qesQuestionService.GetListQuestionByCategoryId(item.Id, 0);
                    Random rnd = new Random();
                    var index = rnd.Next(result.Count);
                    var questionRadom = result[index];
                    // Lấy ra thông tin deviceToken 
                    var devideFirebaseToken = item.DeviceToken;
                    PushNotificationRequest request = new PushNotificationRequest()
                    {
                        notification = new NotificationMessageBody()
                        {
                            title = questionRadom.QuestionName,
                            body = questionRadom.Answer
                        },
                    };
                    request.registration_ids.Add(devideFirebaseToken);
                    await firebaseService.Push(request);
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
