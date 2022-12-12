using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesCommon.EntitiesModel
{
    public class ResponseDicModel
    {
        public string phonetic { get; set; }
        public List<phoneItem> phonetics { get; set; }
    }


    public class phoneItem
    {
        public string text { get; set; }
        public string audio { get; set; }
    }
}
