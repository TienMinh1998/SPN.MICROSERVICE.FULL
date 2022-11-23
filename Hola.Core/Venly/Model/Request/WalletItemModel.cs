using System;

namespace Hola.Core.Venly.Model.Request
{
    public class WalletItemModel
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string WalletType { get; set; }

        public string SecretType { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool Archived { get; set; }

        public string Alias { get; set; }

        public string Description { get; set; }

        public bool Primary { get; set; }

        public bool HasCustomPin { get; set; }

        public string Identifier { get; set; }
        //public Balance Balance { get; set; }
        public long BalanceBalance { get; set; }
        public long GasBalance { get; set; }
        public string Symbol { get; set; }
        public string GasSymbol { get; set; }
        public long RawBalance { get; set; }
        public long RawGasBalance { get; set; }
        public long Decimals { get; set; }
        public bool IsActive { get; set; }

        //public RateCrypto rate { get; set; }
    }
}