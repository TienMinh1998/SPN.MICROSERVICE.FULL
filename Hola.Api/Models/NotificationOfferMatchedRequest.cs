using System;

namespace Hola.Api.Models
{
    public class NotificationOfferMatchedRequest
    {
        public string SellerId { get; set; }
        public string BuyerId { get; set; }
        public string OfferStatus { get; set; }
        public Guid TransactionId { get; set; }
        public string OfferType { get; set; }
    }
}
