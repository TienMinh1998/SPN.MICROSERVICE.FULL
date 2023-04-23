using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hola.Core.Common;
using Hola.Core.Model;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Venly.Model;
using static System.Net.WebRequestMethods;

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

        public async Task<T> GetFromDictionary<T>(string word, string location)  // location đại diện cho ANh anh hay là anh mỹ
        {
            string links = $"https://od-api.oxforddictionaries.com/api/v2/entries/{location}/{word}?strictMatch=false";
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


        public async Task<T> IllustrationImage<T>(string word)  // location đại diện cho ANh anh hay là anh mỹ
        {
            string links = $"https://pixabay.com/api/?key=35427783-15cc811e662e38ffd55809a44&q={word}&image_type=illustration";
            HttpClient client = new HttpClient();
            var request1 = new HttpRequestMessage(HttpMethod.Get, links);
            HttpResponseMessage response = await client.SendAsync(request1);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();
            var Result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return Result;
        }

        public Task<CambridgeDictionaryModel> GetWord(string word)
        {
            CambridgeDictionaryModel response = new CambridgeDictionaryModel();
            try
            {
                // Tạo đối tượng HtmlWeb để tải nội dung của trang web
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load($"https://dictionary.cambridge.org/dictionary/english/{word}");
                var audio = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[1]/div[2]/article/div[2]/div[1]/div[2]/div/div[3]/div/div/div/div[2]/span[1]/span[2]/audio/source[1]");
                var phonetic = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[1]/div[2]/article/div[2]/div[1]/div[2]/div/div[3]/div/div/div/div[2]/span[2]/span[3]/span");
                var x_Type = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[1]/div[2]/article/div[2]/div[1]/div[2]/div/div[3]/div/div/div/div[2]/div[2]/span[1]");
                var x_definition = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[1]/div[2]/article/div[2]/div[1]/div[2]/div/div[3]/div/div/div/div[3]/div/div[2]/div/div[2]/div");
                var x_example = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[1]/div[2]/article/div[2]/div[1]/div[2]/div/div[3]/div/div/div[1]/div[3]/div[1]/div[2]/div[1]/div[3]");


                response.Type = x_Type?.InnerText;
                response.Phonetic = phonetic?.InnerText;
                var mp3url = audio?.Attributes["src"].Value;
                response.Definition = x_definition?.InnerText;
                response.Mp3 = $"https://dictionary.cambridge.org/{mp3url}";
                response.Example = x_example?.InnerText;

            }
            catch (Exception)
            {

                throw;
            }

            return Task.FromResult(response);
        }


        public Task<CambridgeDictionaryVietNamModel> GetVietNamMeaning(string word)
        {
            CambridgeDictionaryVietNamModel response = new CambridgeDictionaryVietNamModel();
            try
            {
                // Tạo đối tượng HtmlWeb để tải nội dung của trang web
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load($"https://dictionary.cambridge.org/dictionary/english-vietnamese/{word}");
                var meaning = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[1]/div[2]/article/div[2]/div[1]/span/div/div[4]/div/div[1]/div[2]/div/div[3]/span");
                response.Meaning = meaning?.InnerText;

            }
            catch (Exception)
            {

                throw;
            }

            return Task.FromResult(response);
        }

        public Task<List<string>> GetSameType(string word)
        {
            List<string> response = new List<string>();
            try
            {
                // Tạo đối tượng HtmlWeb để tải nội dung của trang web
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load($"https://www.oxfordlearnersdictionaries.com/definition/english/{word}?q={word}");

                // Lấy tiêu đề của trang web
                string title = doc.DocumentNode.SelectSingleNode("//title").InnerText;
                Console.WriteLine("Title: " + title);


                Console.WriteLine("Xpath : ");

                var s1 = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div[3]/div[2]/div[2]/div/div[3]/div[1]/dl/dd[1]/ul/li[1]/a");
                var s2 = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div[3]/div[2]/div[2]/div/div[3]/div[1]/dl/dd[1]/ul/li[2]/a");
                var s3 = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div[3]/div[2]/div[2]/div/div[3]/div[1]/dl/dd[1]/ul/li[3]/a");
                if (s1 != null)
                {
                    response.Add(s1.InnerText);
                }
                if (s2 != null)
                {
                    response.Add(s2.InnerText);
                }
                if (s3 != null)
                {
                    response.Add(s3.InnerText);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Task.FromResult(response);
        }


        public async Task<string> UploadFileFromUrlAsync(string rootPath, string fileUrl, string folder)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var httpResponse = await httpClient.GetAsync(fileUrl);

                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        throw new Exception("Failed to download file from URL.");
                    }

                    var fileName = Path.GetFileName(fileUrl);
                    var pathToSave = Path.Combine(rootPath, folder);

                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    var fullPath = Path.Combine(pathToSave, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await httpResponse.Content.CopyToAsync(stream);
                    }
                    var url = $"http://viettienhung.com/images/{fileName}";
                    return await Task.FromResult(url);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}