using System.Collections.Generic;

namespace Hola.Core.Model.DBModel.cmn
{
    public class CurrencyModel
    {
        public int CurrencyId { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }
        public string Number { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public string Units { get; set; }
        public bool IsDeleted { get; set; }
        public List<decimal> listUnits { get; set; }
    }
}
