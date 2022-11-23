using Microsoft.AspNetCore.Http;

namespace Hola.Api.Service.UploadFile.Model
{
    public class baseURLModel
    {
        public IFormFile Files { get; set; }
          public  string FolderName { get; set; }
          public  string base_url {get; set; }
    }
}
