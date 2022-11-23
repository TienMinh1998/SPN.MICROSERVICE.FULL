namespace Hola.Api.Models
{
    public class AgentApplicationFilter
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }
    }
}
