using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hola.Core.Common;
using Hola.Core.Model;

namespace Hola.Core.Helper
{
    public class APICrossHelper
    {
        /// <summary>
        /// Call API Cross Service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="links"></param>
        /// <param name="data"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<T> Post<T>(string links, object data, string token)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);
            var request1 = new HttpRequestMessage(HttpMethod.Post, links);
            request1.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.SendAsync(request1);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();
            var Result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return Result;
        }
       

        public async Task Post(string links, object data, string token)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);
            var request1 = new HttpRequestMessage(HttpMethod.Post, links);
            request1.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            await client.SendAsync(request1);
        }
        public async Task Post(string links, object data)
        {
            HttpClient client = new HttpClient();
            var request1 = new HttpRequestMessage(HttpMethod.Post, links);
            request1.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            await client.SendAsync(request1);
        }

        public async Task Post(string links)
        {
            HttpClient client = new HttpClient();
            var request1 = new HttpRequestMessage(HttpMethod.Post, links);
            await client.SendAsync(request1);
        }

        /// <summary>
        /// author: NVMTien
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="links"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(string links)
        {
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, links);
            HttpResponseMessage response = await client.SendAsync(request);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();

            if (json == null)
                return default(T);

            var Result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return Result;
        }

        public async Task<T> Get<T>(string links, string token)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);
            var request1 = new HttpRequestMessage(HttpMethod.Get, links);
            HttpResponseMessage response = await client.SendAsync(request1);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();
            var Result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return Result;
        }

        public async Task<T> GetFromDictionary<T>(string word)
        {
            string links = $"https://od-api.oxforddictionaries.com/api/v2/entries/en-us/{word}?strictMatch=false";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("app_id", "c4621f80");
            client.DefaultRequestHeaders.Add("app_key", "1d035422e5c1de0548a9fa30f38d7135");
            
            var request1 = new HttpRequestMessage(HttpMethod.Get, links);
            HttpResponseMessage response = await client.SendAsync(request1);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();
            var Result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return Result;
        }
    }
}