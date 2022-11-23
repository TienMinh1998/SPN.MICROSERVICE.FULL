namespace Hola.Api.Models
{
    public class SearchAnnouncementRequest
    {
        public string SearchText { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string OrderDirection { get; set; }
    }
}
