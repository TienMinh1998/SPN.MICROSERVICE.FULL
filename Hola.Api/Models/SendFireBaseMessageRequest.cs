using System.Collections.Generic;

namespace Hola.Api.Models
{
    public class SendFireBaseMessageRequest
    {
        public long RecipientId { get; set; }
        public long NotificationId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int UnreadNoteCount { get; set; }
    }

    public class SendFireBaseDataRequest
    {
        public List<string> RecipientIds { get; set; }
        public string Event { get; set; }
        public string Data { get; set; }
    }

    public class SendForeLogoutRequest
    {
        public string RecipientId { get; set; }
        public string Data { get; set; }
    }
}
