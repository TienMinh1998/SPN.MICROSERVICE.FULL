namespace Hola.Api.Requests.Phrase
{
    public class UpdatePhraseRequest
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Definition { get; set; }
    }
}
