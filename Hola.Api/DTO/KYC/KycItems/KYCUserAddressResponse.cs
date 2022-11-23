using Hola.Api.Model;
using System;
using System.Collections.Generic;

namespace Hola.Api.DTO.KYC.KycItems
{
    public class KYCUserAddressResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Guid KycId { get; set; }
        public short Status { get; set; }
        public short CountryId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string BuildingName { get; set; }
        public string StreetName { get; set; }
        public string ZipCode { get; set; }
        public int FileId { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
        public List<FileModelResponse> documents { get; set; } = new List<FileModelResponse>();
    }
}
