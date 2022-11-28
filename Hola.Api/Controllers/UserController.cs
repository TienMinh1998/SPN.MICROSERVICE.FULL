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


        [Authorize]
        [HttpPost("Register")]
        public async Task<JsonResponseModel> Register([FromBody] UserRegisterRequest request)
        {
            // Check available
            var user = await userService.GetFirstOrDefaultAsync(x => (x.Username.Equals(request.UserName) || x.Email.Equals(request.Email)));
            if (user != null) return JsonResponseModel.Error("Người Dùng đã tồn tại", 400);

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

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username)
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
