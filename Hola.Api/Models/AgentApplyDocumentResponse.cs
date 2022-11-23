namespace Hola.Api.Models
{
    public class AgentApplyDocumentResponse
    {
        public long Id { get; set; }
        public long ApplyId { get; set; }
        public string Name { get; set; }
        public string Ext { get; set; }
        public string FileUrl { get; set; }
        public int DocumentId { get; set; }
    }
}
