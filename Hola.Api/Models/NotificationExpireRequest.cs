using System;
using System.Collections.Generic;

namespace Hola.Api.Models
{
    public class NotificationExpireRequest
    {
        public class User
        {
            public string RecipientId { get; set; }
            public Guid TransactionId { get; set; }
            public string TitleKey { get; set; }
            public string OfferType { get; set; }
            public int UnreadNoteCount { get; set; }
        }
        public List<User> Recipients { get; set; }
    }
}
