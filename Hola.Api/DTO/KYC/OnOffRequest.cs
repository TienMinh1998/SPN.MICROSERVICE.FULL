using System;

namespace Hola.Api.DTO.KYC
{
    public class OnOffRequest
    {
        public string UserId { get; set; }
        public int NewStatus { get; set; }
    }

    public class kycStageRequest
    {
        public Guid KycId { get; set; }
        public int KycStage { get; set; }
    }
}
