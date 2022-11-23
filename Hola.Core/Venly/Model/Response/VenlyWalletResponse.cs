using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venly.Model;


namespace Hola.Core.Venly.Model.Response
{
    public class VenlyWalletResponse
    {
        public bool success { get; set; }
        public List<Wallet> result { get; set; }
    }
}
