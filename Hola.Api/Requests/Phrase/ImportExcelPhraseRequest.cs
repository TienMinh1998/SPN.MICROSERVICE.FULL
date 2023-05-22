using Microsoft.AspNetCore.Http;

namespace Hola.Api.Requests.Phrase
{
    public class ImportExcelPhraseRequest
    {
        public int ReadingId { get; set; }
        public IFormFile file { get; set; }
    }
}
