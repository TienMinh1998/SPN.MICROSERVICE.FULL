using System;
using System.Collections.Generic;

namespace Hola.Api.Models
{
    public class SendNotificationRequest
    {
        public string TitleKey { get; set; }
        public Guid TransactionId { get; set; }
        public string TransactionType { get; set; }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public string NotificationKey { get; set; }
        public decimal? Amount { get; set; }
        public string CurrencySymbol { get; set; }
        public string OfferType { get; set; }
        public int UnreadNoteCount { get; set; }
        public bool MaskPhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public short CurrencyId { get; set; }
        public string? KYCId { get; set; }
        public String? Event { get; set; }
    }

    public class SendKycNotificationRequest
    {
        public string TitleKey { get; set; }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public string NoteText { get; set; }
        public string NoteTitle { get; set; }
        public string NotificationKey { get; set; }
        public DateTime CreatedDate { get; set; }
    }


    public class SendNotificationP2PRequest
    {
        public string TitleKey { get; set; }
        public Guid TransactionId { get; set; }
        public long OfferExchangeId { get; set; }
        public string TransactionType { get; set; }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public string NotificationKey { get; set; }
        public decimal? Amount { get; set; }
        public string CurrencySymbol { get; set; }
        public short Status { get; set; }
        public int UnreadNoteCount { get; set; }
        public bool MaskPhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public short CurrencyId { get; set; }
        public string? KYCId { get; set; }
        public String? Event { get; set; }
    }
    public class SendMultiNotificationRequest
    {
        public string TitleKey { get; set; }
        public Guid TransactionId { get; set; }
        public string TransactionType { get; set; }
        public string SenderId { get; set; }
        public List<string> RecipientId { get; set; }
        public string NotificationKey { get; set; }
        public decimal? Amount { get; set; }
        public string CurrencySymbol { get; set; }
        public string OfferType { get; set; }
        public int UnreadNoteCount { get; set; }
        public bool MaskPhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public short CurrencyId { get; set; }
        public string? KYCId { get; set; }
    }
}
