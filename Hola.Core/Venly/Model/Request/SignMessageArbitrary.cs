using Newtonsoft.Json;
using System.Collections.Generic;
using Venly.Model.Enums;

namespace Venly.Model.Requests
{

    public class CreateWalletRequest
    {
        [JsonProperty("pincode")]
        public long Pincode { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("identifier")]
        public string? Identifier { get; set; }

        [JsonProperty("secretType")]
        public string SecretType { get; set; } 

        [JsonProperty("walletType")]
        public string WalletType { get; set; }
    }

    public class CreateListWalletRequest
    {
        public List<string> SecretType { get; set; }
        public string Pincode { get; set; }
        public string Description { get; set; }
    }
}
