using Hola.Api.Model;
using Hola.Api.Service.UploadFile.Model;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hola.Api.Service.UploadFile
{
    public interface IUploadFileService
    {
        void SettingEvironmentHttpContext(HttpContext value);
        Task<string> UploadImage(IFormFile File);
        Task<string> UploadDocument(IFormFile file_doc);
        Task<string> UploadFile(IFormFile File, string FolerName);
        Task<List<FileItem>> UploadFiles(List<IFormFile> Files, string folder);
        List<string> GetListExtensionSupport();
        Task<FileModel> UploadFile(ClaimsPrincipal User, string fileName, Stream fileData, out string error);
    }
}
