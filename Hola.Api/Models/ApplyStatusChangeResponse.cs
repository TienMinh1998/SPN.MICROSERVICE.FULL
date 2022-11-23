using System;

namespace Hola.Api.Models
{
    public class ApplyStatusChangeResponse
    {
        public int StatusBeforeId { get; set; }
        public string StatusBefore { get; set; }
        public int StatusAfterId { get; set; }
        public string StatusAfter { get; set; }
        public string Reason { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
