using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Hosting;

namespace Hola.Api.Service
{
    public class UploadService : IUploadFileService
    {

        private readonly IWebHostEnvironment _hostEnvironment;

        public UploadService(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folder, HttpContext context)
        {
            try
            {
                var httpRequest = context.Request;
                if (file.Length > 0)
                {
                    var rootPath = _hostEnvironment.WebRootPath != null ? _hostEnvironment.WebRootPath : _hostEnvironment.ContentRootPath;
                    var pathToSave = Path.Combine(rootPath, folder);
                    Console.WriteLine(pathToSave);
                    if (!Directory.Exists(pathToSave)) Directory.CreateDirectory(pathToSave);
                    string name = DateTime.UtcNow.ToString("ssddMMyyyy") + file.FileName;
                    var url = Path.Combine(Directory.GetCurrentDirectory(), $"image/{name}");
                    var fullPath = Path.Combine(pathToSave, name);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return await Task.FromResult(url);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> UploadImage(IFormFile file, HttpContext context)
        {
            try
            {
                var httpRequest = context.Request;
                if (file.Length > 0)
                {
                    var rootPath = _hostEnvironment.WebRootPath != null ? _hostEnvironment.WebRootPath : _hostEnvironment.ContentRootPath;
                    var pathToSave = Path.Combine(rootPath, "image");
                    Console.WriteLine(pathToSave);
                    if (!Directory.Exists(pathToSave)) Directory.CreateDirectory(pathToSave);
                    string name = DateTime.UtcNow.ToString("ssddMMyyyy") + file.FileName;
                    var url = Path.Combine(Directory.GetCurrentDirectory(), $"image/{name}");
                    var fullPath = Path.Combine(pathToSave, name);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    string httpsUrl = $"https://viettienhung.com/images/{name}";
                    return await Task.FromResult(httpsUrl);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
