using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hola.Core.Model.CommonModel
{
    public class CountryRequest
    {
        public int? CountryId { get; set; }
        public string? Name { get; set; }
        public short? CurrencyId { get; set; }
        public string? PhoneCode { get; set; }
        public string? PhonePattern { get; set; }
    }
}
