using Hola.Api.Models.Accounts;
using Hola.Core.Venly.Base;
using Hola.Core.Venly.WalletHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Quartz;
using System.Threading.Tasks;

namespace Hola.Api.Service.Quatz
{
    public class JobClass : IJob
    {

        private readonly IConfiguration _Configuration;
        private readonly AccountService accountService;
        private readonly FirebaseService firebaseService;
        public JobClass(IConfiguration configuration, AccountService accountService, FirebaseService firebaseService)
        {
            _Configuration = configuration;
            this.accountService = accountService;
            this.firebaseService = firebaseService;
        }
        public async Task CheckTaskService()
        {
            try
            {
                PushNotificationRequest request = new PushNotificationRequest()
                {
                    notification = new NotificationMessageBody()
                    {
                        body = $"JboClass Test",
                        title = "iobClass Test"
                    }
                };
                request.registration_ids.Add("dhz36LPnR9WWj48VAweHFb:APA91bGPqyu0F6eu4N1JMg1d9DesPfXGIINfmxJm-zauEKmGq3_XSGZR49NvBeo4vdXUFM6OxcThnDrYYe4sPmy3awx0AJVq92VqtPalqD5PnfjWlL_5C_6IlymAdTeEaLj3E06r6AeT");
                await firebaseService.Push(request);
            }
            catch (System.Exception)
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
