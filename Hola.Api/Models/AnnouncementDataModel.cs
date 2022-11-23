using System;
using System.Collections.Generic;

namespace Hola.Api.Models
{
    public class AnnouncementDataModel
    {
        public long AnnouncementId { get; set; }
        public short LanguageId { get; set; }
        public short CountryId { get; set; }
        public long CategoryId { get; set; }
        public string Title { get; set; }
        public string Html { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsMainAnnouncement { get; set; }
        public bool IsPosted { get; set; }
    }
}
