using Microsoft.AspNetCore.Http;

namespace Hola.Api.Models
{
    public class FeatureNotificationModel
    {
        public long Id { get; set; }
        //Login user
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public short LanguageId { get; set; }
        public string ImageUrl { get; set; }
    }
}
