using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesCommon.Requests.GrammarRequests
{
    public class AddGrammarRequest
    {
        public string grammar_name { get; set; }
        public string grammar_content { get; set; }
        public string Code { get; set; }
    }
}
