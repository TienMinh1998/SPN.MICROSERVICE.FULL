using System;
using System.Collections.Generic;

namespace Hola.Api.Models
{
    public class NotificationFilterModel
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<String>? TitleKey { get; set; }
    }
}
