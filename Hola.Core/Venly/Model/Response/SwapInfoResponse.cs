using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venly.Model.Response
{
    public class SwapInfoResponse
    {
        public Token From { get; set; } = new Token() { SecretType = Enums.SecretType.ETHEREUM, Symbol = "NA", TokenAddress = "0x00" };
        public Token To { get; set; } = new Token() {  SecretType = Enums.SecretType.ETHEREUM, Symbol = "NA", TokenAddress = "0x00"};
    }
}
