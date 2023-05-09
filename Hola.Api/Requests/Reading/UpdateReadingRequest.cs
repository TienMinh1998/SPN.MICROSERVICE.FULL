namespace Hola.Api.Requests.Reading
{
    public class UpdateReadingRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Definetion { get; set; }
        public string Content { get; set; }
        public string Translate { get; set; }
        public string Status { get; set; }
    }
}
