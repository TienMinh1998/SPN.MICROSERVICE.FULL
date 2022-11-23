using System;

namespace Hola.Api.Models
{
    public class NotificationTransactionStatusChangedRequest
    {
        public string SellerId { get; set; }
        public string BuyerId { get; set; }
        public Guid TransactionId { get; set; }
        public string OfferType { get; set; }
    }
}
