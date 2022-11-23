using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hola.Core.Model.CommonModel
{
    public class CoreSendFireBaseDataRequest
    {
            public List<string> RecipientIds { get; set; }
            public string Event { get; set; }
            public string Data { get; set; }
    }
}
