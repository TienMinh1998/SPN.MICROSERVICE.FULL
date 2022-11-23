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



        [HttpPost("Login")]
        public async Task<JsonResponseModel> Search()
        {
            return JsonResponseModel.Success();
        }
    }
}
