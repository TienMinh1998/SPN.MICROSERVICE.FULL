using System.Collections.Generic;

namespace Hola.Api.Models
{
    public class SetNotificationAsReadRequest
    {
        public List<int> NotificationIds { get; set; }
    }

    //public class SetAsInactiveRequest
    //{
    //    public long RecipientId { get; set; }
    //    public long TransactionId { get; set; }
    //    public string Action { get; set; }
    //}
}
