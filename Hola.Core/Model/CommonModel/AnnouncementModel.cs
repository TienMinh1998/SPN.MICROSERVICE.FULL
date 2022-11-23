using System;
using System.Collections.Generic;

namespace Hola.Core.Model.CommonModel
{
    public class AnnouncementModel
    {
        public long AnnouncementId { get; set; }
        public short LanguageId { get; set; }
        public short CountryId { get; set; }
        public long CategoryId { get; set; }
        public string Html { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public List<string> UserIdsInCountry { get; set; }
    }
}
