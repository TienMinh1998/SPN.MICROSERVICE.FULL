using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesCommon.Requests.TargetRequests
{
    public class AddTargetRequest
    {
        public string target_content { get; set; }
        public string description { get; set; }
        public int total_days { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public DateTime created_on { get; set; }
    }
}
