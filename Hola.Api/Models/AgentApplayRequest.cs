using System.Collections.Generic;

namespace Hola.Api.Models
{
    public class AgentApplayDocumentRequest
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public long Id { get; set; }
        public string FileUrl { get; set; }
        public int Size { get; set; }
    }
    public class AgentApplayRequest
    {
        public List<AgentApplayDocumentRequest> Documents { get; set; }
    }
}
