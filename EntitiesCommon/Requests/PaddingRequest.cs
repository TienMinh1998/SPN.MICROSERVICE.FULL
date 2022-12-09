using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesCommon.Requests
{
    public class PaddingRequest
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
