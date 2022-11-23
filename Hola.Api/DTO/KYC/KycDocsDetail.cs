namespace Hola.Api.DTO.KYC
{
    public class KycDocsDetail
    {
        public short BankInfoStatus { get; set; }
        public short DocsStatus { get; set; }
        public short DeclarationsStatus { get; set; }
        public short UserAddressStatus { get; set; }
        public short UserInfoStatus { get; set; }
        public int IDCardBack { get; set; }
        public int IDCardFont { get; set; }
        public int ImageId { get; set; }
        public int BankStatement { get; set; }
        public int NetWorthUsDollars { get; set; }
        public int AttachAfterSigning { get; set; }
        public int AddressFileID { get; set; }
    }
}
