using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venly.Model.Enums;

namespace Venly.Model
{
    public class Token
    {
        [JsonProperty("secretType")]
        public SecretType SecretType { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; } = String.Empty;

        [JsonProperty("tokenAddress")]
        public string TokenAddress { get; set; } = String.Empty;
    }
}
