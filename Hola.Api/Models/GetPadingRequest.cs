namespace Hola.Api.Models
{
    public class GetPadingRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string ColumnSort { get; set; }
        public bool IsDesc { get; set; }
    }
}
