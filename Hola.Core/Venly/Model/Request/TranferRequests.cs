using Newtonsoft.Json;


namespace Hola.Core.Venly.Model.Request
{
    public class TranferRequest
    {
        [JsonProperty("pincode")]
        public int Pincode { get; set; }
        [JsonProperty("transactionRequest")]
        public TransactionRequest transactionRequest { get; set; }
    }

    public class TransactionRequest
    {
        [JsonProperty("type")]
        public string type { get; set; }  // Defailt Tranfer
        [JsonProperty("walletId")]
        public string walletId { get; set; }

        [JsonProperty("to")]
        public string to { get; set; }  // WalletAddress
        [JsonProperty("secretType")]
        public string secretType { get; set; } // Coin Name
        [JsonProperty("value")]
        public decimal value { get; set; }
        public string data { get; set; } = null;
    }

    public class TranferRequestApp
    {
        [JsonProperty("pincode")]
        public string Pincode { get; set; }
        [JsonProperty("toWalletAddress")]
        public string ToWalletAddress { get; set; }  // WalletAddress
        public string secretType { get; set; } // Coin Name
        public decimal Value { get; set; }
    }
}
