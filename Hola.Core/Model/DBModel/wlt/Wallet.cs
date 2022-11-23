using System;

namespace Hola.Core.Model.DBModel.wlt
{
    public class Wallet
    {
        public Guid WalletId { set; get; }
        public string UserId { set; get; }
        public short CurrencyId { set; get; }
        public decimal Balance { set; get; }
        public decimal BlockedBalance { set; get; }
        public int Status { set; get; }
        public bool IsDefault { set; get; }
        public DateTime CreateDate { set; get; }
    }
}