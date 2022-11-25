using System;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    public class UserController : ControllerBase
    {
        public UserController()
        {
                
        }

        [HttpPost("LoginUser")]
        public async Task<JsonResponseModel> Search()
        {
            return JsonResponseModel.Success();
        }
    }
}
