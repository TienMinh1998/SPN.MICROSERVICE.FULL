using System;
using Newtonsoft.Json;
using Venly.Model.Response;

namespace Hola.Core.Venly.Model.Request
{
    public class ListWallet
    {
        public Guid Id { get; set; }
        public int currencyCryptoId { get; set; }
        public string Address { get; set; }
        public string WalletType { get; set; }
        public string SecretType { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Archived { get; set; }
        public string Alias { get; set; }
        public string logo { get; set; }
        public string Description { get; set; }
        public bool Primary { get; set; }
        public bool HasCustomPin { get; set; }
        public string Identifier { get; set; }
        public long balance { get; set; }
        public long GasBalance { get; set; }                                  
        public string Symbol { get; set; }
        public string GasSymbol { get; set; }
        public long RawBalance { get; set; }
        public long RawGasBalance { get; set; }
        public long Decimals { get; set; }
        public bool IsActive { get; set; }
        public bool status { get; set; }
        public decimal USD { get; set; }
        public decimal KRW { get; set; }
    }

    public class WalletItem
    {
        public Guid id { get; set; }
        public decimal VenlyBalance { get; set; }
        public int currencyCryptoid { get; set; }
        public string address { get; set; }
        public string walletType { get; set; }
        public string secretType { get; set; }
        public DateTime createdAt { get; set; }
        public bool archived { get; set; }
        public string Alias { get; set; }
        public string Logo { get; set; }
        public string description { get; set; }
        public bool primary { get; set; }
        public bool hasCustomPin { get; set; }
        public string Identifier { get; set; }
        public decimal balance { get; set; }
        public decimal gasBalance { get; set; }
        public string symbol { get; set; }
        public string gasSymbol { get; set; }
        public long rawBalance { get; set; }
        public long rawGasBalance { get; set; }
        public long decimals { get; set; }
        public bool IsActive { get; set; }
        public bool Status { get; set; }
        //public RateCrypto rate { get; set; }
        public decimal USD { get; set; }
        public decimal KRW { get; set; }
    }

}