using System;

namespace Hola.Api.Models
{
    public class AnnouncementClientModel
    {
        public long AnnouncementId { get; set; }
        public DateTime Date { get; set; }
        public string Html { get; set; }
        public bool Read { get; set; }
    }
}
