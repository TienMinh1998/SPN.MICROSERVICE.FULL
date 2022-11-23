using Newtonsoft.Json;

namespace Hola.Core.Venly.Model.Response
{
    public class RateItemWallet
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public string logoUrl { get; set; }

        public price Price { get; set; }
    }

    public partial class price
    {
        [JsonProperty("EUR")]
        public long eur { get; set; }

        [JsonProperty("PLN")]
        public long pln { get; set; }

        [JsonProperty("USD")]
        public long usd { get; set; }

        [JsonProperty("GBP")]
        public long gbp { get; set; }
    }
}