using Hola.Api.Models.Accounts;
using Hola.Api.Models.Categories;
using Hola.Api.Service;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sentry.Protocol;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Hola.Api.Controllers.FireBaseController;

namespace Hola.Api.Controllers
{
    public class FireBaseController : ControllerBase
    {



        [HttpPost("PushMessage")]
        public async Task<JsonResponseModel> Push(PushNotificationRequest pushNotificationRequest)
        {
            FirebaseService firebaseService = new FirebaseService();
            var result =await  firebaseService.Push(pushNotificationRequest);
            return JsonResponseModel.Success(result);
        }
     
    }
}
