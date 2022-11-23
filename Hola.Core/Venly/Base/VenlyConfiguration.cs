using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hola.Core.Venly.Base
{
    public class VenlyConfiguration
    {
        public string GrantType { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AppId { get; set; }
        public string AuthenUrl { get; set; }
        public string WalletEndPoint { get; set; }
        public string MarketEndPoint { get; set; }
        public string NFTEndPoint { get; set; }
    }


}
