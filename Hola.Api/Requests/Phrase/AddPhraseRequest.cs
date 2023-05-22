namespace Hola.Api.Requests.Phrase
{
    public class AddPhraseRequest
    {
        public int ReadingId { get; set; }
        public string word { get; set; }
        public string Meaning { get; set; }
    }
}
