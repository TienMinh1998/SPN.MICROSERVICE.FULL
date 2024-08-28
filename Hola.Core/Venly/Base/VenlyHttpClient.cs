using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Venly.Model;

namespace Hola.Core.Venly.Base
{
    public class VenlyHttpClient : HttpClient
    {
        protected VenlyConfiguration _venlyConfiguration;
        public VenlyHttpClient(string accessToken, VenlyConfiguration venlyConfiguration)
        {
            DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            BaseAddress = new Uri(venlyConfiguration.WalletEndPoint);
            _venlyConfiguration = venlyConfiguration;
        }

        public async Task<VenlyResponseBase<T>?> GetAsync<T>(string url)
        {
            try
            {
                var getResponse = await this.GetAsync(url);
                if (getResponse.IsSuccessStatusCode)
                {
                    var data = new VenlyResponseBase<T>();
                    var content = await getResponse.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<VenlyResponseBase<T>>(content);

                    if (data != null) return data;
                    if (data == null) return new VenlyResponseBase<T> { Success = false, Exception = new Exception(content) };
                }
                else
                {
                    var content = await getResponse.Content.ReadAsStringAsync();
                    return new VenlyResponseBase<T> { Success = false, Exception = new Exception(content) };
                }
                return null;
            }
            catch (Exception ex)
            {
                return new VenlyResponseBase<T>()
                {
                    Success = false,
                    Exception = ex
                };
            }
        }

        public async Task<VenlyResponseBase<T>?> PostAsync<T>(string url, string requestPayload)
        {
            try
            {
                var getResponse = await PostAsync(url, new StringContent(requestPayload, Encoding.UTF8, "application/json"));
                if (getResponse.IsSuccessStatusCode)
                {
                    var data = new VenlyResponseBase<T>();
                    var content = await getResponse.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<VenlyResponseBase<T>>(content);
                    if (data != null) return data;
                    if (data == null) return new VenlyResponseBase<T> { Success = false, Exception = new Exception(content) };
                }
                else
                {
                    var content = await getResponse.Content.ReadAsStringAsync();
                    return new VenlyResponseBase<T> { Success = false, Exception = new Exception(content) };
                }
                return null;
            }
            catch (Exception ex)
            {
                return new VenlyResponseBase<T>()
                {
                    Success = false,
                    Exception = ex
                };
            }
        }

        public async Task<VenlyResponseBase<T>?> PatchAsync<T>(string url, string requestPayload)
        {
            try
            {
                var getResponse = await PatchAsync(url, new StringContent(requestPayload, Encoding.UTF8, "application/json"));
                if (getResponse.IsSuccessStatusCode)
                {
                    var data = new VenlyResponseBase<T>();
                    var content = await getResponse.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<VenlyResponseBase<T>>(content);
                    if (data != null) return data;
                    if (data == null) return new VenlyResponseBase<T> { Success = false, Exception = new Exception(content) };
                }
                else
                {
                    var content = await getResponse.Content.ReadAsStringAsync();
                    return new VenlyResponseBase<T> { Success = false, Exception = new Exception(content) };
                }
                return null;
            }
            catch (Exception ex)
            {
                return new VenlyResponseBase<T>()
                {
                    Success = false,
                    Exception = ex
                };
            }
        }

        public async Task<T> GetAsyncT<T>(string url)
        {
            try
            {
                var getResponse = await this.GetAsync(url);
                if (getResponse.IsSuccessStatusCode)
                {
                    var content = await getResponse.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<T>(content);

                    if (data != null) return data;
                    if (data == null) return default(T);
                }
                return default(T);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public async Task<T> PostAsyncGeneric<T>(string url, string requestPayload)
        {
            try
            {
                var getResponse = await PostAsync(url, new StringContent(requestPayload, Encoding.UTF8, "application/json"));
                if (true)
                {
                    var content = await getResponse.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<T>(content);
                    if (data != null) return data;
                    if (data == null) return default(T);
                }
                else
                {
                    return default(T);
                }
                return default(T);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

    }
}
