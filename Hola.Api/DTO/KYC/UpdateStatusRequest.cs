using System;

namespace Hola.Api.DTO.KYC
{
    public class UpdateStatusRequest
    {
        public int DocumentCheckType { get; set; }
        public int Status { get; set; }
        public string UserId { get; set; }
        public string Note { get; set; }
        public string RejectReason { get; set; }

    }
    public enum DocumentCheckType
    {
        KYCBankInfo = 0,
        KYCDeclarations = 1,
        KYCDocs = 2,
        KYCUserAddress = 3,
        KYCUserInfo = 4
    }
}
