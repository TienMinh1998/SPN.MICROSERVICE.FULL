using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hola.Api.Models
{
    public class AgentApplicationSearchRequest
    {
        public string SearchText { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public short? CountryId { get; set; }
        public short? StatusId { get; set; }
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; } //ASC or DESC

      
    }
}
