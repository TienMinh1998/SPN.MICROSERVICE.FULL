using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venly.Model
{

    public class Wallet
    {

        public Guid Id { get; set; }
        public string Address { get; set; }
        public string WalletType { get; set; }
        public string SecretType { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool Archived { get; set; }

        public string Alias { get; set; }

        public string Description { get; set; }

        public bool Primary { get; set; }

        public bool HasCustomPin { get; set; }

        public string Identifier { get; set; }
        public Balance balance { get; set; }
    }

    public partial class Balance
    {
        [JsonProperty("available")]
        public bool available { get; set; }

        [JsonProperty("secretType")]
        public string? secretType { get; set; }

        [JsonProperty("balance")]
        public decimal balance { get; set; }

        [JsonProperty("gasBalance")]
        public decimal gasBalance { get; set; }

        [JsonProperty("symbol")]
        public string? symbol { get; set; }

        [JsonProperty("gasSymbol")]
        public string? gasSymbol { get; set; }

        [JsonProperty("rawBalance")]
        public long rawBalance { get; set; }

        [JsonProperty("rawGasBalance")]
        public long rawGasBalance { get; set; }

        [JsonProperty("decimals")]
        public long decimals { get; set; }
    }
}
