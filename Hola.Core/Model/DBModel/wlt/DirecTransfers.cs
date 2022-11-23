using Hola.Core.Common;
using Hola.Core.Model.DBModel.cmn;

namespace Hola.Core.Model.DBModel.wlt
{
    public class DirecTransfers
    {
        public long Id { set; get; }
        public string sourceUserId { set; get; }
        public string destUserId { set; get; }
        public CurrencyModel currency { set; get; }
        public decimal amount { set; get; }
        public string Notes { set; get; }
        public string sourcePhoneN { set; get; }
        public string destPhoneN { set; get; }
        public string sourceCardHolderName { set; get; }
        public string destinationCardHolderName { set; get; }
    }
}