using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hola.Api.Models
{
    public class NotificationAction
    {
        [JsonIgnore]
        public long NotificationId { get; set; }
        public string Name { get; set; }
        public string Parametr { get; set; }
        public string ParametrName { get; set; }
        public bool IsActive { get; set; }
    }

    public class SearchNotificationResponse
    {
        public long Id { get; set; }
        public long Date { get; set; }
        public string Html { get; set; }
        public bool Read { get; set; }
        public List<NotificationAction> Actions { get; set; }
    }


}
