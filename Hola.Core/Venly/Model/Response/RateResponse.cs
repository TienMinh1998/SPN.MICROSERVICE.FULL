using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venly.Model.Response
{
    public partial class RateResponse
    {
        [JsonProperty("exchangeRates")]
        public List<Rate> ExchangeRates { get; set; } = new List<Rate>();

        [JsonProperty("bestRate")]
        public Rate? BestRate { get; set; }
    }

    public partial class Rate
    {
        [JsonProperty("exchange")]
        public string Exchange { get; set; } = string.Empty;

        [JsonProperty("orderType")]
        public string OrderType { get; set; } = string.Empty;

        [JsonProperty("inputAmount")]
        public long InputAmount { get; set; }

        [JsonProperty("outputAmount")]
        public double OutputAmount { get; set; }

        [JsonProperty("slippage")]
        public double Slippage { get; set; }

        [JsonProperty("fee")]
        public long Fee { get; set; }
    }

}
