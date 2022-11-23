using Hola.Api.Model;
using System;
using System.Collections.Generic;

namespace Hola.Api.DTO.KYC.KycItems
{
    public class KycDeclarationResponse
    {
        public short  Status { get; set; }
        public string UserId { get; set; }
        public Guid KycId { get; set; }
        public int AttachAfterSigning { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
        public List<FileModelResponse> documents { get; set; } = new List<FileModelResponse>();
    }
}
