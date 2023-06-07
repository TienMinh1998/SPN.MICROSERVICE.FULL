using Hola.Api.Models;

namespace Hola.Api.Requests.Reading
{
    public class SearchReadingRequest : BaseSearchModel
    {
        public int? Type { get; set; }
    }
}
