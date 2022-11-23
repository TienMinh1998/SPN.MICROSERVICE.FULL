using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hola.Api.Models
{
    public class AgencyManagementHistoryRequest
    {
        public string UserID { get; set; }
        public int PageNumber { get; set; }
        public int PageLimit { get; set; }
    }
}
