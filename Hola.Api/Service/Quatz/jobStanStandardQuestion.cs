using DatabaseCore.Domain.Entities.Normals;
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
        private readonly IQuestionStandardService _questionStandardService;
        private readonly IUserService _userServices;
        private readonly FirebaseService firebaseService;
        private readonly DapperBaseService _dapper;

        public jobStanStandardQuestion(IQuestionStandardService questionStandardService, IUserService userServices, FirebaseService firebaseService, DapperBaseService dapper)
        {
            _questionStandardService = questionStandardService;
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

                    string queryRandomQuestion = "select * from public.\"QuestionStandards\" where \"Pk_QuestionStandard_Id\" " +
                        "=(SELECT usr.random_between(1,(select count(1)::integer from public.\"QuestionStandards\")))  and \"IsDeleted\" = false ;";

                    var question = _dapper.QueryFirst<QuestionStandard>(queryRandomQuestion);
                    PushNotificationRequest request = new PushNotificationRequest()
                    {
                        notification = new NotificationMessageBody()
                        {
                            title = $"{question.English}",
                            body = $"{question.Phonetic}"
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
