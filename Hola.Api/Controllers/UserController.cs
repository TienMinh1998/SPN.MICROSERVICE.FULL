using System;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Hola.Api.Service;
using Hola.Api.Models.Accounts;

namespace Hola.Api.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly AccountService accountService;

        public UserController(AccountService accountService)
        {
            this.accountService = accountService;
        }

   

        [HttpPost("LoginUser")]
        public async Task<JsonResponseModel> Search()
        {
            return JsonResponseModel.Success();
        }


        [HttpPost("UpdateDeviceToken")]
        public async Task<JsonResponseModel> UpdateDeviceToken(UpdateDeviceTokenRequest updateRequest)
        {
            // Get result From service
            try
            {
                var resultService = await accountService.UpdateDeviceTokenFirebaseAsync(updateRequest.DeviceToken, updateRequest.UserId);
                return JsonResponseModel.Success(resultService);
            }
            catch (Exception)
            {
                return JsonResponseModel.SERVER_ERROR();
            }
        }
    }
}
