using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesCommon.EntitiesModel
{
    public class TargetModel
    {
        public int PK_TargetId { get; set; }
        public int FK_UserId { get; set; }
        public string target_content { get; set; }
        public string description { get; set; }
        public int total_days { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public DateTime created_on { get; set; }
    }
}
