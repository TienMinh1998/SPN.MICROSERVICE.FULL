using Hola.Api.Models.Accounts;
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
        public jobStanStandardQuestion(IQuestionStandardService questionStandardService, IUserService userServices, FirebaseService firebaseService)
        {
            _questionStandardService = questionStandardService;
            _userServices = userServices;
            this.firebaseService = firebaseService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                // User nào bật thông báo mới có
                var listUser = await _userServices.GetAllAsync(x => (x.isnotification == 1 && x.IsDeleted !=1));
                var response = listUser.ToList();
                foreach (var item in response)
                {
                    // Lấy ra thông tin deviceToken 
                    string userName = item.Name;
                    var devideFirebaseToken = item.DeviceToken;
                    var count =await _questionStandardService.CountAsync(x=>x.IsDeleted!=true);
                    Random rnd = new Random();
                    var index = rnd.Next(count-1);
                    var question = await _questionStandardService.GetFirstOrDefaultAsync(x=>x.Pk_QuestionStandard_Id==index);   
                    PushNotificationRequest request = new PushNotificationRequest()
                    {
                        notification = new NotificationMessageBody()
                        {
                            title = $"{question.English} {question.Phonetic}",
                            body = $"{question.MeaningVietNam}"
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
