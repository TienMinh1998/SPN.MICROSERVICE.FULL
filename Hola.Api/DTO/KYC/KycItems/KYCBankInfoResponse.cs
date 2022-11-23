using Hola.Api.Model;
using System;
using System.Collections.Generic;

namespace Hola.Api.DTO.KYC.KycItems
{
    public class KYCBankInfoResponse
    {
        public short Status { get; set; }
        public string UserId { get; set; }
        public Guid KycId { get; set; }
        //----------------------------------------------
        public string AccountHolderName { get; set; }
        public string BankName { get; set; }
        public string BankCountryDetail { get; set; }
        public string BankBranchName { get; set; }
        public string AccountNumber { get; set; }
        public string BankCountryStatement { get; set; }
        public int BankStatement { get; set; }
        public string EmploymentStatus { get; set; }
        public int NetWorthUsDollars { get; set; }
        public string InfoOnSourceAndUseOfFunds { get; set; }
        public string Note { get; set; }
        //------------------------------------------------
        public List<FileModelResponse> documents { get; set; } = new List<FileModelResponse>();
    }
}
