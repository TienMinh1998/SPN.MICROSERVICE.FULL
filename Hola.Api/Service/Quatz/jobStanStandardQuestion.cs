using DatabaseCore.Domain.Questions;
using Hola.Api.Models.Accounts;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.UserServices;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hola.Api.Service.Quatz
{
    public class jobStanStandardQuestion : IJob
    {

        private readonly IUserService _userServices;
        private readonly FirebaseService firebaseService;
        private readonly DapperBaseService _dapper;

        public jobStanStandardQuestion(IUserService userServices, FirebaseService firebaseService, DapperBaseService dapper)
        {

            _userServices = userServices;
            this.firebaseService = firebaseService;
            _dapper = dapper;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                // User nào bật thông báo mới có
                var listUser = await _userServices.GetAllAsync(x => (x.isnotification == 1 && x.IsDeleted != 1));
                var response = listUser.ToList();
                foreach (var item in response)
                {
                    // Lấy ra thông tin deviceToken 
                    string userName = item.Name;
                    var devideFirebaseToken = item.DeviceToken;

                    string queryRandomQuestion = "SELECT * FROM (SELECT * FROM \"QuestionStandards\" qs WHERE qs.created_on >= (NOW() - INTERVAL '2 MONTH') order by created_on desc ) sub ORDER BY random() LIMIT 1;";

                    var question = _dapper.QueryFirst<QuestionStandard>(queryRandomQuestion);
                    PushNotificationRequest request = new PushNotificationRequest()
                    {
                        notification = new NotificationMessageBody()
                        {
                            title = $"{question.English}",
                            body = $"{question.Note}"
                        }
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
