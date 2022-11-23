using System.Collections.Generic;

namespace Hola.Core.Model.DBModel.cmn
{
    public class CountryModel
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public short CurrencyId { get; set; }
        public string PhoneCode { get; set; }
        public string PhonePattern { get; set; }
        public string ISO2 { get; set; }
        public int FlagId { get; set; }
        public string LimitDigitPhoneNumber { get; set; }
        public List<decimal> ListDigit { get; set; }
    }
}
