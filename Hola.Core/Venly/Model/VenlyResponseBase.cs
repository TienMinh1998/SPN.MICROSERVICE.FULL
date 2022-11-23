using Newtonsoft.Json;
using System;

namespace Venly.Model
{
    public class VenlyResponseBase<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        public T? Result { get; set; }
        public Exception? Exception { get; set; }
    }

}