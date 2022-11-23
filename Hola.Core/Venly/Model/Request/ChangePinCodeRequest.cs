using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hola.Core.Venly.Model.Request
{
    public class ChangePinCodeRequest
    {
        public string pincode { get; set; }
        public string newPincode { get; set; }
    }
}
