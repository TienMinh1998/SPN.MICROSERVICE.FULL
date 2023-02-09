using AutoMapper;
using Hola.Api.Models;
using Hola.Api.Service;
using Hola.Api.Service.GrammarServices;
using Hola.Api.Service.UserManualServices;
using Hola.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Hola.Api.Service.BaseServices;

namespace Hola.Api.Controllers
{
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notiservice;
        private readonly IMapper _mapperService;
        private readonly DapperBaseService _dapper;
        public NotificationController(INotificationService notiservice,
            IMapper mapperService,
            DapperBaseService dapper = null)

        {
            _notiservice = notiservice;
            _mapperService = mapperService;
            _dapper = dapper;
        }


        [HttpGet("Notification")]
        [Authorize]
        public async Task<JsonResponseModel> GetAll()
        {
            try
            {
                // Lấy ra thông tin thông báo của User
                int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var result = await _notiservice.GetAllAsync(x => x.FK_UserId == userid && x.IsRead == false);
                var list_order_by = result.OrderByDescending(x => x.created_on).Take(30);
                return JsonResponseModel.Success(list_order_by);
            }
            catch (System.Exception Ex)
            {
                return JsonResponseModel.SERVER_ERROR(Ex.Message);

            }
        }

        [HttpPost("watched")]
        [Authorize]
        public async Task<JsonResponseModel> Watched([FromBody] ReadNotificationRequest model)
        {
            try
            {
                // Lấy ra thông tin thông báo của User
                int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var noti = await _notiservice.GetFirstOrDefaultAsync(x => x.FK_UserId == userid && x.IsRead == false && x.Pk_Id == model.PK_notification_Id);
                if (noti != null)
                {
                    noti.IsRead = true;
                    var updateResponse = await _notiservice.UpdateAsync(noti);
                    return JsonResponseModel.Success(updateResponse);
                }
                else
                {
                    return JsonResponseModel.Success("Đọc thông báo không thành công");
                }
            }
            catch (System.Exception Ex)
            {
                return JsonResponseModel.SERVER_ERROR(Ex.Message);

            }
        }

        /// <summary>
        /// Đọc tất cả các thông báo
        /// </summary>
        /// <returns></returns>
        [HttpPost("watchedAll")]
        [Authorize]
        public async Task<JsonResponseModel> Watched()
        {
            try
            {
                // Lấy ra thông tin thông báo của User
                int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                string query = $"UPDATE usr.\"Notification\" set \"IsRead\"=true WHERE \"FK_UserId\" = {userid}";
                await _dapper.Execute(query);
                return JsonResponseModel.Success("Đánh dấu đã đọc tất cả các thông báo thành công");

            }
            catch (System.Exception Ex)
            {
                return JsonResponseModel.SERVER_ERROR(Ex.Message);

            }
        }
    }
}
