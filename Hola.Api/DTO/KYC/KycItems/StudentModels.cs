using Microsoft.AspNetCore.Http;

namespace Hola.Api.DTO.KYC.KycItems
{
    public class StudentModels
    {
        public string Name { get; set; }
        public int  Age { get; set; }
        public string Address  { get; set; }
        public IFormFile Files { get; set; }
    }
}
