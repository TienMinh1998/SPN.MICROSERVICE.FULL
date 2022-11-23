using System;
using System.Collections.Generic;

namespace Hola.Api.Models
{
    public class PushNotificationModel
    {
        public long NotificationId { get; set; }
        public string RecipientId { get; set; }
        public Guid TransactionId { get; set; }
        public string Title { get; set; }
        public string Html { get; set; }
        public bool Read { get; set; }
        public DateTime CreatedDate { get; set; }
        public string TitleKey { get; set; }
        public string TransactionType { get; set; }
        public string NotificationKey { get; set; }
        public int NotificationCount { get; set; }
        public string TransactionStatus { get; set; }
        public int AnnouncementCount { get; set; }
        public string OfferType { get; set; }
        public List<string> Events { get; set; }
        public decimal? Amount { get; set; }
        public string CurrencySymbol { get; set; }
        public short CurrencyId { get; set; }
    }

    public class PushNotificationP2PModel
    {
        public long NotificationId { get; set; }
        public string RecipientId { get; set; }
        public Guid TransactionId { get; set; }
        public long OfferExchangeId { get; set; }
        public string Title { get; set; }
        public string Html { get; set; }
        public bool Read { get; set; }
        public DateTime CreatedDate { get; set; }
        public string TitleKey { get; set; }
        public string TransactionType { get; set; }
        public string NotificationKey { get; set; }
        public int NotificationCount { get; set; }
        public string TransactionStatus { get; set; }
        public int AnnouncementCount { get; set; }
        public string OfferType { get; set; }
        public List<string> Events { get; set; }
        public decimal? Amount { get; set; }
        public string CurrencySymbol { get; set; }
        public short CurrencyId { get; set; }
    }
}
