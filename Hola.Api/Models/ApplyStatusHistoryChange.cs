using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hola.Api.Models
{
    public class ApplyStatusHistoryChange
    {
        public int StatusId { get; set; }
        public string Reason { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
