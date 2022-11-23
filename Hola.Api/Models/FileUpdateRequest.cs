using Microsoft.AspNetCore.Http;

namespace Hola.Api.Models
{
    public class FileUpdateRequest
    {
        public long Id { get; set; }
        public IFormFile FileToUpload { get; set; }
    }
}
