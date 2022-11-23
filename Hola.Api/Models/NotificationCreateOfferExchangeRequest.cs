using System;

namespace Hola.Api.Models
{
    public class NotificationCreateOfferExchangeRequest
    {
        public string SellerId { get; set; }
        public string BuyerId { get; set; }
        public Guid TransactionId { get; set; }
        public long OfferExchangeId { get; set; }
        public short Status { get; set; }
        public string NotifiKey { get; set; }
    }
}