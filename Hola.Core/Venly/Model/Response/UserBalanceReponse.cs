using System.Collections.Generic;

namespace Hola.Core.Venly.Model.Response
{
    public class UserBalanceReponse
    {
        public bool success { get; set; }
        public IList<string> result { get; set; }
    }
}