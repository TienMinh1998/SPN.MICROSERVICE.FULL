using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hola.Core.Model.CommonModel
{
    public class DataResult
    {
        public IList Items { get; set; }
        public DataResult()
        {

        }
        public DataResult(IList list)
        {
            Items = list;
        }
    }
}
