using System;

namespace Hola.Api.DTO.KYC.KycItems
{
    public class KycHistoryItemResponse
    {
        public int Id { get; set; }
        public Guid KycId { get; set; }
        public string Type { get; set; }
        public int StatusBefore { get; set; }
        public int StatusAfter { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangeBy { get; set; }
        public string Reason { get; set; }
        public string StepName { get; set; }
    }
}
