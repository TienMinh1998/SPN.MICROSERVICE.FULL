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
        public async Task<JsonResponseModel> Push([FromBody] PushNotificationRequest pushNotificationRequest)
        {
            string url = "https://fcm.googleapis.com/fcm/send";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=" + "AAAAWttKm9g:APA91bHIhzkBBitMVDWALEEaGLSrPd5Bpjv_qYUx0DZ9RlKdR9_Va-oQXR1eTvMf2D7iMnTUGfD5-4eN2kaupnOy1RDf8aA6pa98KXdGGjhm4HAiZZU9-YptL27JCmr1quYpwwQ9dn3M");
                string serializeRequest = JsonConvert.SerializeObject(pushNotificationRequest);
                var response = await client.PostAsync(url, new StringContent(serializeRequest, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return JsonResponseModel.Success();
                }
                return JsonResponseModel.SERVER_ERROR();
            }
        }
        public class PushNotificationRequest
        {
            public List<string> registration_ids { get; set; } = new List<string>();
            public NotificationMessageBody notification { get; set; }
            public object data { get; set; }
        }

        public class NotificationMessageBody
        {
            public string title { get; set; }
            public string body { get; set; }
        }
    }
}
