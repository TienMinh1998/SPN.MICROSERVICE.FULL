namespace Hola.Api.Models
{
    public class GetPadingRequest
    {
        public int? courseId { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string columnSort { get; set; }
        public bool isDesc { get; set; }
    }
}
