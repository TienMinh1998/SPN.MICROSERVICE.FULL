using System.Collections.Generic;

namespace Hola.Api.Models
{
    public class NotificationAdminKYCRequest
    {
        public string SenderId { get; set; }
        public List<string> RecipientId { get; set; }
        public string? KYCId { get; set; }
    }
}
