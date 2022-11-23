using System.Collections.Generic;

namespace Hola.Core.Venly.Model.Response
{
    public class WalletAddressResponse
    {
        public bool success { get; set; }
        public IList<string> result { get; set; }
    }
}
