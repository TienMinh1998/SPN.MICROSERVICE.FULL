using Hola.Api.Models;
using Hola.Api.Service;
using Hola.Core.Common;
using Hola.Core.Model;
using Hola.Core.Model.CommonModel;
using Hola.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class AnnouncementController : ControllerBase
    {
        // GET
        private readonly ILogger<AnnouncementController> _logger;

        private readonly IOptions<SettingModel> _config;

        public AnnouncementController(ILogger<AnnouncementController> logger, IOptions<SettingModel> config)
        {
            _logger = logger;
            _config = config;
        }

        /// <summary>
        /// LVTan
        /// Search Announcement
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [HttpPost("search")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public JsonResponseModel Search(AnnouncementFilterModel filters)
        {
            JsonResponseModel response = new JsonResponseModel();

            try
            {
                _logger.LogInformation("Search Announcement");
                AnnouncementService _service = new AnnouncementService(_config);
                var data = _service.Search(filters);
                if (data != null)
                {
                    response.Status = 200;
                    response.Data = data;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Data = new DataResultsModel();
                    response.Message = Constant.MSG_DATA_NOT_FOUND;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Search Announcement: " + ex.Message);
                response.Status = 100;
                response.Data = new DataResultsModel();
                response.Message = Constant.MSG_DATA_NOT_FOUND;
            }

            return response;
        }

        /// <summary>
        /// LVTan
        /// Create Announcement
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<JsonResponseModel> Create(AnnouncementModel model)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Create Announcement");
                AnnouncementService _service = new AnnouncementService(_config);
                FireBaseService fireBaseService = new FireBaseService(_config);
                var data = await _service.Create(model);
                if (data != null)
                {
                    response.Status = 200;
                    response.Data = data;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Data = false;
                    response.Message = Constant.MSG_ERROR;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Create Announcement: " + ex.Message);
                response.Status = 100;
                response.Data = false;
                response.Message = Constant.MSG_ERROR;
            }

            return response;
        }



        [HttpPost("PostToUser")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<JsonResponseModel> PostToUser(AnnouncementModel model)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Post To User");
                AnnouncementService _service = new AnnouncementService(_config);
                FireBaseService fireBaseService = new FireBaseService(_config);
                var data = await _service.PostToUser(model, Request.Headers["Authorization"].ToString());
                if (data != null)
                {
                    List<KeyValuePair<string, long>> anouList = new List<KeyValuePair<string, long>>();
                    foreach (var a in data.UserAnnouncementMap)
                    {
                        anouList.Add(KeyValuePair.Create(a.Key, a.Value));
                    }
                    PingService.AddAnnouncement(anouList);

                    var dataSendFireBase = new SendFireBaseDataRequest
                    {
                        RecipientIds = anouList.Select(c => c.Key).ToList(),
                        Event = Constants.FireBase.EventAnnouncementCount
                    };
                    fireBaseService.SendAnnoutcementData(dataSendFireBase, model);
                    response.Status = 200;
                    response.Data = true;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Data = false;
                    response.Message = Constant.MSG_ERROR;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Post To User: " + ex.Message);
                response.Status = 100;
                response.Data = false;
                response.Message = Constant.MSG_ERROR;
            }

            return response;
        }

        [HttpPost("UnpostToUser")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<JsonResponseModel> UnPostToUser(long announcementId)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Unpost To User");
                AnnouncementService _service = new AnnouncementService(_config);
                FireBaseService fireBaseService = new FireBaseService(_config);
                var data = _service.UnPostToUser(announcementId);
                if (data != null)
                {
                    List<KeyValuePair<string, long>> anouList = new List<KeyValuePair<string, long>>();
                    foreach (var a in data)
                    {
                        PingService.RemoveAnnouncementForUser(a, new List<long> { announcementId });
                    }
                    response.Status = 200;
                    response.Data = true;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Data = false;
                    response.Message = Constant.MSG_ERROR;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unpost To User: " + ex.Message);
                response.Status = 100;
                response.Data = false;
                response.Message = Constant.MSG_ERROR;
            }

            return response;
        }

        /// <summary>
        /// LVTan
        /// Update Announcement
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost("update")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public JsonResponseModel Update(AnnouncementModel model)
        {
            JsonResponseModel response = new JsonResponseModel();

            try
            {
                _logger.LogInformation("Update Announcement");
                AnnouncementService _service = new AnnouncementService(_config);
                bool result = _service.Update(model);
                if (result)
                {
                    response.Status = 200;
                    response.Data = true;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Data = false;
                    response.Message = Constant.MSG_UPDATE_FALSE;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Update Announcement: " + ex.Message);
                response.Status = 100;
                response.Data = false;
                response.Message = Constant.MSG_UPDATE_FALSE;
            }

            return response;
        }

        [HttpPost("UpdateIsMainAnnouncement")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public JsonResponseModel UpdateIsMainAnnouncement(bool model, long announcementId)
        {
            JsonResponseModel response = new JsonResponseModel();

            try
            {
                _logger.LogInformation("Update Is Main Announcement");
                AnnouncementService _service = new AnnouncementService(_config);
                bool result = _service.UpdateIsMainAnnouncement(model, announcementId);
                if (result)
                {
                    response.Status = 200;
                    response.Data = true;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Data = false;
                    response.Message = Constant.MSG_UPDATE_FALSE;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Update Is Main Announcement: " + ex.Message);
                response.Status = 100;
                response.Data = false;
                response.Message = Constant.MSG_UPDATE_FALSE;
            }

            return response;
        }

        /// <summary>
        /// LVTan
        /// Delete Announcement
        /// </summary>
        /// <param name="curUserId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public JsonResponseModel Delete(string curUserId, long id)
        {
            JsonResponseModel response = new JsonResponseModel();

            try
            {
                _logger.LogInformation("Delete Announcement with id : " + id);
                AnnouncementService _service = new AnnouncementService(_config);
                bool result = _service.Delete(curUserId, id);
                if (result)
                {
                    response.Status = 200;
                    response.Data = true;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Data = false;
                    response.Message = Constant.MSG_UPDATE_FALSE;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete Announcement: " + ex.Message);
                response.Status = 100;
                response.Data = false;
                response.Message = Constant.MSG_UPDATE_FALSE;
            }

            return response;
        }

        /// <summary>
        /// LVTan
        /// Set As Read
        /// </summary>
        /// <param name="curUserId"></param>
        /// <param name="announcementIds"></param>
        /// <returns></returns>
        [HttpPost("SetAsRead")]
        [Authorize]
        public JsonResponseModel SetAsRead(List<long> announcementIds)
        {
            JsonResponseModel response = new JsonResponseModel();

            try
            {
                _logger.LogInformation("SetAsRead: " + string.Join(",", announcementIds));
                AnnouncementService _service = new AnnouncementService(_config);
                bool result = _service.SetAsRead(announcementIds,User);
                if (result)
                {
                    response.Status = 200;
                    response.Data = true;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Data = false;
                    response.Message = Constant.MSG_UPDATE_FALSE;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("SetAsRead: " + ex.Message);
                response.Status = 100;
                response.Data = false;
                response.Message = Constant.MSG_UPDATE_FALSE;
            }

            return response;
        }

        [HttpPost("user")]
        [Authorize]
        public JsonResponseModel UserSearch(SearchAnnouncementRequest request)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("UserSearch");
                AnnouncementService service = new AnnouncementService(_config);
                var data = service.GetUserAnnouncement(request, User);
                if (data != null)
                {
                    response.Status = 200;
                    response.Data = data;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Message = Constant.MSG_UPDATE_FALSE;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("UserSearch: " + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_ERROR;
            }
            return response;
        }
        
        [HttpGet("getUnreadCount")]
        [Authorize]
        public JsonResponseModel GetUnreadCount()
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("getUnreadCount");
                AnnouncementService service = new AnnouncementService(_config);
                var data = service.GetUnReadCount(User);
                response.Status = 200;
                response.Data = data;
                response.Message = Constant.MSG_SUCCESS;
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("getUnreadCount: " + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_ERROR;
            }
            return response;
        }
        
        [HttpPost("SetAsReadAll")]
        [Authorize]
        public JsonResponseModel SetAsReadAll()
        {
            JsonResponseModel response = new JsonResponseModel();

            try
            {
                _logger.LogInformation("SetAsReadALL");
                AnnouncementService _service = new AnnouncementService(_config);
                bool result = _service.SetAsReadAll(User);
                if (result)
                {
                    response.Status = 200;
                    response.Data = true;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Data = false;
                    response.Message = Constant.MSG_UPDATE_FALSE;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("SetAsRead: " + ex.Message);
                response.Status = 100;
                response.Data = false;
                response.Message = Constant.MSG_UPDATE_FALSE;
            }

            return response;
        }
    }
}