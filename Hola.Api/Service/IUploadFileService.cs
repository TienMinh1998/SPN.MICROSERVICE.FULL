using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;
using System;

namespace Hola.Api.Service
{
    public interface IUploadFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folder, HttpContext context);
        Task<string> UploadImage(IFormFile file, HttpContext context);
    }
}
