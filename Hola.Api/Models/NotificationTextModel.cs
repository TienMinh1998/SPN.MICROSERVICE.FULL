namespace Hola.Api.Models
{
    public class NotificationTextModel
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string PushText { get; set; }
        public bool PushFireBase { get; set; }
    }
}
