using Hola.Api.Model;
using System;
using System.Collections.Generic;

namespace Hola.Api.DTO.KYC.KycItems
{
    public class KYCDocsResponse
    {
        public short Status { get; set; }
        public string UserId { get; set; }
        public Guid KycId { get; set; }

        //------------------------------------
        public short DocumentType { get; set; }
        public int IDCardBack { get; set; }
        public int IDCardFont { get; set; }
        public int ImageId { get; set; }
        public string Note { get; set; }
        public List<FileModelResponse> documents { get; set; } = new List<FileModelResponse>();
    }
}
