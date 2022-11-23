using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hola.Core.Venly.Model.Response
{
    public class ChainsResponse
    {
        public bool success { get; set; }
        public IList<string> result { get; set; }
    }
}
