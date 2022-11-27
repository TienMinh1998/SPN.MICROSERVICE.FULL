using System;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Hola.Api.Service;
using Hola.Api.Models.Accounts;
using Hola.Api.Service.UserServices;

namespace Hola.Api.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly AccountService accountService;
        private readonly IUserService userService;
        public UserController(AccountService accountService, IUserService userService)
        {
            this.accountService = accountService;
            this.userService = userService;
        }



        [HttpPost("LoginUser")]
        public async Task<JsonResponseModel> Search()
        {
            return JsonResponseModel.Success();
        }


        [HttpPost("UpdateDeviceToken")]
        public async Task<JsonResponseModel> UpdateDeviceToken([FromBody] UpdateDeviceTokenRequest updateRequest)
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

        [HttpPost("GetUsers")]
        public async Task<JsonResponseModel> GetUsers()
        {
            // Get result From service
            try
            {
               var response =await userService.GetAllAsync();
                return JsonResponseModel.Success(response);
            }
            catch (Exception)
            {
                return JsonResponseModel.SERVER_ERROR();
            }
        }
    }
}
