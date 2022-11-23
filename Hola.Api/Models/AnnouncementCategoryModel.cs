using System;

namespace Hola.Api.Models
{
    public class AnnouncementCategoryModel
    {
        public int AnnouncementCategoryId { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
