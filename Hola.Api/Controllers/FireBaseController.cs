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
        private readonly FirebaseService _service;
        public FireBaseController(FirebaseService service = null)
        {
            _service = service;
        }

        [HttpPost("PushMessage")]
        public async Task<JsonResponseModel> Push([FromBody] PushNotificationRequest pushNotificationRequest)
        {
            var result = await _service.Push(pushNotificationRequest, 1);
            return JsonResponseModel.Success(result);
        }

    }
}
