namespace Hola.Api.Models
{
    public class GetStandQuestionRequest
    {
        public int TargetID { get; set; }
        public int pageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
