using Microsoft.AspNetCore.Http;

namespace Hola.Api.Models
{
    public class NotificationModel
    {
        //Login user
        public string UserId { get; set; }
        public string Token { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public short? LanguageId { get; set; }
        public IFormFile FileToUpload { get; set; }
        public bool IsSendFireBase { get; set; }
        public string FilePath { get; set; }
    }
}
