using Hola.Api.Models.Accounts;
using Hola.Core.Venly.Base;
using Hola.Core.Venly.WalletHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hola.Api.Service.Quatz
{
    public class JobClass : IJob
    {

        private readonly IConfiguration _Configuration;
        private readonly AccountService accountService;
        private readonly FirebaseService firebaseService;
        private readonly QuestionService qesQuestionService;
        public JobClass(IConfiguration configuration, AccountService accountService, FirebaseService firebaseService, QuestionService qesQuestionService)
        {
            _Configuration = configuration;
            this.accountService = accountService;
            this.firebaseService = firebaseService;
            this.qesQuestionService = qesQuestionService;
        }
        public async Task CheckTaskService()
        {
            try
            {
                var result = await qesQuestionService.GetListQuestionByCategoryId(1, 0);
                Random rnd = new Random();
                var index = rnd.Next(result.Count);
                var questionRadom = result[index];
                // Lấy ra thông tin deviceToken 
                var devideFirebaseToken = accountService.GetDeviceTokenByUserId(1);
                PushNotificationRequest request = new PushNotificationRequest()
                {
                    notification = new NotificationMessageBody()
                    {
                        title = questionRadom.QuestionName+ " <3",
                        body = "*******?"
                    }
                };
                request.registration_ids.Add(devideFirebaseToken);
                await firebaseService.Push(request);
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
