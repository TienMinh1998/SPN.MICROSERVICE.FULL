using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hola.Core.Venly.Model.Response
{
    public class SendCryptoResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        public List<error> errors { get; set; }

    }
    public class error
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
