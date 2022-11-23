using Hola.Api.Model;
using System;
using System.Collections.Generic;

namespace Hola.Api.DTO.KYC.KycItems
{
    public class KYCUserInfoResponse
    {
        public short Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public short Gender { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
        public string RejectReason { get; set; }
        public List<FileModelResponse> documents { get; set; } = new List<FileModelResponse>();
    }
}
