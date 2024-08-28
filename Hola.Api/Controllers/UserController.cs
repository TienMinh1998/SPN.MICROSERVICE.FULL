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
using EntitiesCommon.Requests;
using Hola.Api.Response;
using Hola.Api.Service.V1;
using Microsoft.AspNetCore.Http;
using System.IO;
using Hola.GoogleCloudStorage;
using Sentry;
using Hola.Api.Service.BaseServices;
using Hola.Api.Authorize;
using MediatR;
using SPNApplication.Queries;

namespace Hola.Api.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly AccountService accountService;
        private readonly IUserService userService;
        private readonly IConfiguration _configuration;
        private readonly IQuestionService _questionService;
        private readonly Service.QuestionService daper_questionService;
        public readonly IUploadFileGoogleCloudStorage _GoogleCloudStorage;
        private readonly DapperBaseService _dapper;
        private readonly IMediator _mediator;
        public UserController(AccountService accountService,
            IUserService userService, IConfiguration configuration,
            IQuestionService questionService, Service.QuestionService daper_questionService,
            IUploadFileGoogleCloudStorage googleCloudStorage = null, DapperBaseService dapper = null, IMediator mediator = null)
        {
            this.accountService = accountService;
            this.userService = userService;
            _configuration = configuration;
            _questionService = questionService;
            this.daper_questionService = daper_questionService;
            _GoogleCloudStorage = googleCloudStorage;
            _dapper = dapper;
            _mediator = mediator;
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
            DatabaseCore.Domain.Entities.Normals.User addUser = new DatabaseCore.Domain.Entities.Normals.User
            {
                PhoneNumber = request.Phone,
                Username = request.UserName,
                Email = email,
                Name = request.Name,
                Password = passwordHash

            };
            var add_user = await userService.AddAsync(addUser);
            // PHân quyền luôn cho User là một người dùng thông thường
            string queryaddUserRole = $"select * from usr.create_user_role({add_user.Id}, {(int)USERROLE.NORMAR_USER})";
            await _dapper.Execute(queryaddUserRole);
            // Todo
            return JsonResponseModel.Success(addUser);
        }
        /// <summary>
        /// Đăng nhập App
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<JsonResponseModel> Login([FromBody] LoginRequest request)
        {
            var user = await userService.GetFirstOrDefaultAsync(x => x.Username.Equals(request.UserName));

            if (user != null)
            {
                var isPasswordOk = BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.Password, BCrypt.Net.HashType.SHA384);
                if (isPasswordOk)
                {
                    user.DeviceToken = request.DevideToken;
                    var userUpdateDevice = await userService.UpdateAsync(user);
                    var new_token = await _mediator.Send(new LoginApplicationQuery
                    {
                        passwork = request.Password,
                        username = user.Username,
                        user_id = user.Id,
                    });
                    LoginResponse loginResponse = new LoginResponse
                    {
                        Token = new_token,
                        user = user
                    };
                    return JsonResponseModel.Success(loginResponse);
                }
            }
            return JsonResponseModel.Error("Sai tên đăng nhập hoặc mật khẩu", 401);

        }
        /// <summary>
        /// Đăng nhập admin
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Login_Admin")]
        public async Task<JsonResponseModel> LoginAdmin([FromBody] LoginRequestAdmin request)
        {
            var user = await userService.GetFirstOrDefaultAsync(x => x.Username.Equals(request.UserName));
            if (user != null)
            {
                var isPasswordOk = BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.Password, BCrypt.Net.HashType.SHA384);
                if (isPasswordOk)
                {
                    // Tạo Quền cho người dùng 
                    var userID = user.Id;
                    string sql = "SELECT p.\"Id\"  FROM usr.\"User\" u \r\nINNER JOIN usr.\"UserRole\" ur ON u.\"Id\" = ur.\"FK_UserID\" " +
                        "INNER JOIN usr.rolepermission rp ON ur.\"FK_RoleID\" = rp.\"FK_RoleID\" " +
                        $"INNER JOIN usr.\"permission\" p ON rp.\"FK_PermissionID\" = p.\"Id\" WHERE u.\"Id\" = {userID} ";
                    var permistion = (await _dapper.GetAllAsync<string>(sql)).ToArray();

                    var newToken = CreateToken(user, permistion);
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


        [HttpGet("OverView")]
        [Authorize]
        public async Task<JsonResponseModel> Overview()
        {
            try
            {
                int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var totalQuestion = await _questionService.CountAsync(x => x.fk_userid == userid);
                var learned = await _questionService.CountAsync(x => (x.fk_userid == userid && x.is_delete == 1));
                var notLearnd = await _questionService.CountAsync(x => (x.fk_userid == userid && x.is_delete == 0));
                var totaltoday = await daper_questionService.CountQuestionToday(userid);

                OverViewModel model = new OverViewModel()
                {
                    Total = totalQuestion,
                    TotalLearned = learned,
                    TotalNotLearnd = notLearnd,
                    TotalToday = totaltoday
                };
                return JsonResponseModel.Success(model);
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
        [HttpPost("On_Notification")]
        [Authorize]
        public async Task<JsonResponseModel> On_Notification([FromBody] ChangeNotificationRequest action)
        {
            // Get result From service
            try
            {
                var userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == SystemParam.CLAIM_USER).Value);
                var user = await userService.GetFirstOrDefaultAsync(x => x.Id == userid);
                if (action.TurnOn == true)
                {
                    // If action equal 'true' => on Notification 
                    user.isnotification = 1;
                    await userService.UpdateAsync(user);
                }
                else
                {
                    // If action equal 'false' => Off Notification
                    user.isnotification = 0;
                    await userService.UpdateAsync(user);
                }
                return JsonResponseModel.Success(action);
            }
            catch (Exception)
            {
                return JsonResponseModel.SERVER_ERROR();
            }
        }
        /// <summary>
        /// tạo lịch sử học tập theo ngày
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetHistoryCurrentDay")]
        [Authorize]
        public async Task<JsonResponseModel> GetHistoryCurrentDay()
        {
            // Get result From service
            try
            {
                var userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == SystemParam.CLAIM_USER).Value);
                var result = await accountService.CreateHistoryOneDay(userid, 10);
                if (result == true)
                {
                    return JsonResponseModel.Success(new List<string>(), $"Tạo lịch sử ngày {DateTime.Now.ToString("yyyy-MM-dd")} thành công");
                }
                else
                {
                    return JsonResponseModel.Error("Tạo lịch sử không thành công", 500);
                }
            }
            catch (Exception)
            {
                return JsonResponseModel.SERVER_ERROR();
            }
        }

        [HttpPost("UploadAvartar")]
        public async Task<JsonResponseModel> UploadAvartar(IFormFile? inputFiles)
        {
            var userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == SystemParam.CLAIM_USER).Value);
            var user = await userService.GetFirstOrDefaultAsync(x => x.Id == userid);
            if (user != null)
            {
                var filename = inputFiles;
                var filePath = Path.GetTempFileName();
                using (var stream = System.IO.File.Create(filePath))
                {
                    // The formFile is the method parameter which type is IFormFile
                    // Saves the files to the local file system using a file name generated by the app.
                    await inputFiles.CopyToAsync(stream);
                }
                var resultUrl = _GoogleCloudStorage.UploadFile(filePath, "5512421445", inputFiles.FileName, "credentials.json", "image", "image/jpeg");
                user.Avartar = resultUrl;
                await userService.UpdateAsync(user);
                return JsonResponseModel.Success(resultUrl);
            }
            return JsonResponseModel.SERVER_ERROR("Cập nhật ảnh đại diện thất bại");
        }

        private string CreateToken(DatabaseCore.Domain.Entities.Normals.User user, string[] permission = null)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim("UserId", user.Id.ToString())
            };

            if (permission != null && permission.Length > 0)
            {
                foreach (var item in permission)
                {
                    claims.Add(new Claim(JwtClaimsTypes.Role, item));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JWT:Secret").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
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
