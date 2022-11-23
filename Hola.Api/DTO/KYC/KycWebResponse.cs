// Author : Criss 
// Date : 22-06-2022
using Hola.Api.DTO.KYC.KycItems;
using System;

namespace Hola.Api.DTO.KYC
{
    public class KycWebResponse
    {
        public short step { get; set; }
        public short Status { get; set; }
        public int RoleId { get; set; }
        public short KycStage { get; set; }
        public short TransactionTimeOfUser { get; set; }
        public short CountryId { get; set; }
        public Guid KycId { get; set; }
        public string UserId { get; set; }
        public string Username  { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public  DateTime ModifiedDate { get; set; }
    }
    public class KycDetailWebResponse
    {
        public string Note { get; set; }
        public long KycStage { get; set; }
        public long Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public KYCBankInfoResponse BankInfomation { get; set; }
        public KYCDocsResponse DocsInfomation  { get; set; }
        public KycDeclarationResponse DeclarationInfomation { get; set; }
        public KYCUserInfoResponse  UserInfomation { get; set; }
        public KYCUserAddressResponse Adress { get; set; }
    }

    public enum KYCSStageEnum
    {
        Suspended = 0,
        Pending = 1,
        Approved = 2,
        Rejected = 3,
    }

    public class KYCs
    {
        public short KycStage { get; set; }
        public short Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public String Note { get; set; }
    }
}
