using System;

namespace Hola.Api.Models
{
    public class NotificationAdminTransferRequest
    {
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencySymbol { get; set; }
        public short CurrencyId { get; set; }
    }


}
