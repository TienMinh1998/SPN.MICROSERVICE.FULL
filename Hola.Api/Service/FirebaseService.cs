using Hola.Api.Models.Accounts;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hola.Api.Service
{
    public class FirebaseService
    {

      /// <summary>
      /// Push Message
      /// </summary>
      /// <param name="pushNotificationRequest"></param>
      /// <returns></returns>
        public async Task<bool> Push(PushNotificationRequest pushNotificationRequest)
        {
            string url = "https://fcm.googleapis.com/fcm/send";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=" + "AAAAWttKm9g:APA91bHIhzkBBitMVDWALEEaGLSrPd5Bpjv_qYUx0DZ9RlKdR9_Va-oQXR1eTvMf2D7iMnTUGfD5-4eN2kaupnOy1RDf8aA6pa98KXdGGjhm4HAiZZU9-YptL27JCmr1quYpwwQ9dn3M");
                string serializeRequest = JsonConvert.SerializeObject(pushNotificationRequest);
                var response = await client.PostAsync(url, new StringContent(serializeRequest, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                return false;
            }
        }
      
    }
}
