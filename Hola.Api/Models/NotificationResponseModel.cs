using System;
using System.Collections.Generic;

namespace Hola.Api.Models
{
    public class NotificationResponseModel
    {
        public long NotificationId { get; set; }
        public string RecipientId { get; set; }
        public Guid TransactionId { get; set; }
        public long OfferExchangeId { get; set; }
        public string Title { get; set; }
        public string Html { get; set; }
        public bool Read { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string TitleKey { get; set; }
        public string SenderId { get; set; }
        public string TransactionType { get; set; }
        public string NotificationKey { get; set; }
        public string OfferType { get; set; }
        public short LanguageId { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionDirection { get; set; }
        public List<string> Actions { get; set; }

    }
}
