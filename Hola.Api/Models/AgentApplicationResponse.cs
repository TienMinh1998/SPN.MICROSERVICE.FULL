using System;
using System.Collections.Generic;

namespace Hola.Api.Models
{
    public class AgentApplicationResponse
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string User { get; set; }
        public int CountryId { get; set; }
        public string UserPhone { get; set; }
        public byte StatusId { get; set; }
        public DateTime AppliedDate { get; set; }
        public int DocumentId { get; set; }
        public IList<AgentApplyDocumentResponse> Documents { get; set; } = new List<AgentApplyDocumentResponse>();
    }
}
