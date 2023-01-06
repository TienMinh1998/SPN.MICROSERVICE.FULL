namespace Hola.Api.Requests
{
    public class PaddingQuestionRequest
    {
        public int Category_Id { get; set; }
        public string SortColumn { get; set; }
        public bool IsDesc { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class WordPadingRequest
    {
        public string SearchKey { get; set; }
        public string SortColumn { get; set; }
        public bool IsDesc { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
