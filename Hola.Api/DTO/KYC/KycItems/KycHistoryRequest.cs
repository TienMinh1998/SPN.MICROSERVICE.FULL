using System;

namespace Hola.Api.DTO.KYC.KycItems
{
    public class KycHistoryRequest
    {
        public Guid KycId { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
