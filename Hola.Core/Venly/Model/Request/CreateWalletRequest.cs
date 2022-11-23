using Newtonsoft.Json;
using System;
using Venly.Model.Enums;

namespace Venly.Model.Requests
{
    public class ChamCham
    {
        [JsonProperty("pincode")]
        public string Pincode { get; set; } = String.Empty;

        [JsonProperty("signatureRequest")]
        public SignatureRequest? Request { get; set; }
    }

    public class SignatureRequest
    {
        [JsonProperty("type")]
        public string Type { get; set; } = String.Empty;

        [JsonProperty("secretType")]
        public SecretType SecretType { get; set; } = SecretType.ETHEREUM;

        [JsonProperty("walletId")]
        public Guid WalletId { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; } = String.Empty;
    }
}
