using System;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Hola.Api.Service;
using Hola.Api.Models.Accounts;
using Hola.Api.Service.UserServices;
using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Requests.Users;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Hola.Api.Response.Jwt;
using Hola.Api.Response.Login;
using System.Linq;
using Hola.Api.Common;

namespace Hola.Api.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly AccountService accountService;
        private readonly IUserService userService;
        private readonly IConfiguration _configuration;

        public UserController(AccountService accountService, 
            IUserService userService, IConfiguration configuration)
        {
            this.accountService = accountService;
            this.userService = userService;
            _configuration = configuration;
        }

        /// <summary>
        /// Đăng kí người dùng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<JsonResponseModel> Register([FromBody] UserRegisterRequest request)
        {
            // Check available
            var user = await userService.GetFirstOrDefaultAsync(x => (x.Username.Equals(request.UserName)));
            if (user != null) return JsonResponseModel.Error("Người Dùng đã tồn tại! vui lòng thử 1 UserName Khác.", 500);

            var user1 = await userService.GetFirstOrDefaultAsync(x => (x.Email.Equals(request.Email)));
            if (user1 != null) return JsonResponseModel.Error("Email này đã có người sử dụng, vui lòng thử 1 Email khác", 500);

            string userName = request.UserName;
            var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password, 11);
            string email = request.Email;
            User addUser = new User
            {
                
                PhoneNumber = request.Phone,
                Username = request.UserName,
                Email = email,
                Name = request.Name,
                Password = passwordHash
               
            };
            await userService.AddAsync(addUser);
            return JsonResponseModel.Success(addUser);
        }

        [HttpPost("Login")]
        public async Task<JsonResponseModel> Login([FromBody] LoginRequest request)
        {
            var user = await userService.GetFirstOrDefaultAsync(x => x.Username.Equals(request.UserName));
            if (user!=null)
            {
                var isPasswordOk = BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.Password, BCrypt.Net.HashType.SHA384);
                if (isPasswordOk)
                {
                    // Update Devide Token of User 
                    user.DeviceToken = request.DevideToken;
                    var userUpdateDevice =await userService.UpdateAsync(user); 
                    // Tạo Token và trả cho người dùng
                    var newToken = CreateToken(user);
                    LoginResponse loginResponse = new LoginResponse
                    {
                        Token = newToken,
                        user = user
                    };
                    return JsonResponseModel.Success(loginResponse);
                }
            }
            return JsonResponseModel.Error("Sai tên đăng nhập hoặc mật khẩu", 401);

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

        /// <summary>
        /// Bật thông báo
        /// </summary>
        /// <param name="updateRequest"></param>
        /// <returns></returns>
        [HttpGet("On_Notification")]
        [Authorize]
        public async Task<JsonResponseModel> On_Notification(int Status)
        {
            // Get result From service
            try
            {
                var userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == SystemParam.CLAIM_USER).Value);
                var user = await userService.GetFirstOrDefaultAsync(x => x.Id == userid);
                if (Status==1)
                {
                    // bật thông báo
                    user.isnotification = 1;
                    await userService.UpdateAsync(user);
                }
                else
                {
                    // Không thông báo nữa
                    user.isnotification = 0;
                    await userService.UpdateAsync(user);
                }
                return JsonResponseModel.Success(Status);
            }
            catch (Exception)
            {
                return JsonResponseModel.SERVER_ERROR();
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim("UserId", user.Id.ToString())
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("JWT:Secret").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha384Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3),
                signingCredentials: cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

    }


   

}
