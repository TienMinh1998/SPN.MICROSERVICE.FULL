using System;

namespace Hola.Core.Model.DBModel.deal
{
    public class Transaction
    {
        public Guid TransId { set; get; }
        public long OfferId { set; get; }
        public short CurrencyId { set; get; }
        public string SellerId { set; get; }
        public string BuyerId { set; get; }
        public string Type { set; get; }
        public decimal RequestedAmount { set; get; }
        public decimal AvailableAmount { set; get; }
        public decimal DealAmount { set; get; }
        public string Status { set; get; }
        public DateTime TransferDate { set; get; }
        public bool IsDeleted { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public DateTime ModifiedDate { set; get; }
        public string ModifiedBy { set; get; }

    }
}