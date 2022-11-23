using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hola.Api.Models
{
    public class AnnouncementResponse
    {
        public long CategoryId { get; set; }
        public string Category { get; set; }
        public short CountryId { get; set; }
        public short LanguageId { get; set; }
        public string Html { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsPosted { get; set; }
        public bool IsMainAnnouncement { get; set; }
        public List<KeyValuePair<string,long>> UserAnnouncementMap { get; set; }
        public string UserId { get; set; }
        public int AnnouncementId { get; set; }
    }
}
