using System;

namespace Hola.Api.Models
{
    public class FeatureNotificationDataModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public short LanguageId { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsSend { get; set; }
    }
}
