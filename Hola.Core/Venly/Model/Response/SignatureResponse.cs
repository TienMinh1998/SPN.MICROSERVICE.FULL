using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venly.Model
{
    public class SignatureResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; } = String.Empty;

        [JsonProperty("r")]
        public string R { get; set; } = String.Empty;

        [JsonProperty("s")]
        public string S { get; set; } = String.Empty;

        [JsonProperty("v")]
        public string V { get; set; } = String.Empty;

        [JsonProperty("signature")]
        public string Signature { get; set; } = String.Empty;
    }
}
