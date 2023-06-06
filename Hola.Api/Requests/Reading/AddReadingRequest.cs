using Microsoft.AspNetCore.Http;

namespace Hola.Api.Requests.Reading
{
    public class AddReadingRequest
    {
        public string Title { get; set; }
        public string Definetion { get; set; }
        public IFormFile file { get; set; }
        public string Content { get; set; }
        public string Translate { get; set; }
        public string TaskName { get; set; }
        public int Band { get; set; }
        public int Type { get; set; }
    }
}
