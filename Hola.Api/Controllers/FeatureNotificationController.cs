using Hola.Api.Models;
using Hola.Api.Service;
using Hola.Core.Common;
using Hola.Core.Model;
using Hola.Core.Model.CommonModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class FeatureNotificationController : ControllerBase
    {
        // GET
        private readonly ILogger<FeatureNotificationController> _logger;
        private readonly IOptions<SettingModel> _config;

        public FeatureNotificationController(ILogger<FeatureNotificationController> logger, IOptions<SettingModel> config)
        {
            _logger = logger;
            _config = config;
        }
        [HttpPost]
        [Route("Sendfirebasedemo")]
        public JsonResponseModel SendFireBaseDEMO(string userId)
        {

            JsonResponseModel response = new JsonResponseModel();
            FireBaseService _service = new FireBaseService(_config);
            var msg = new Message
            {
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = "title",
                    Body = "body",
                },

                Apns = new ApnsConfig { Aps = new Aps { Sound = "default", Badge = 1 } },
                Data = new Dictionary<string, string>()
                {
                    { "NotificationId", 1.ToString() },
                },
            };
            _service.SendFirebase(msg);
            return response;
        }

        /// <summary>
        /// LVTan
        /// Search Feature Notification
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [HttpPost("search")]
        public JsonResponseModel Search(FeatureNotificationFilterModel filters)
        {
            JsonResponseModel response = new JsonResponseModel();

            try
            {
                _logger.LogInformation("Search Feature Notification");                
                FeatureNotificationService _service = new FeatureNotificationService(_config);
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
                _logger.LogInformation(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Search Feature Notification: " + ex.Message);
                response.Status = 100;
                response.Data = new DataResultsModel();
                response.Message = Constant.MSG_DATA_NOT_FOUND;
            }

            return response;
        }


        [HttpPost("SendToUser")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<JsonResponseModel> SendToUser(FeatureNotificationModel model)
        {
            JsonResponseModel response = new JsonResponseModel();

            try
            {
                _logger.LogInformation("Create Feature Notification");
                FeatureNotificationService _service = new FeatureNotificationService(_config);
                var data = await _service.SendToUser(model.Id,model.LanguageId, Request.Headers["Authorization"].ToString());
                FireBaseService fireBaseService = new FireBaseService(_config);
                if (data != null)
                {
          
                    var dataSendFireBase = new SendFireBaseDataRequest
                    {
                        RecipientIds = data.Select(c => c.Key).ToList(),
                        Event = Constants.FireBase.EventFeatureCount
                    };
                    fireBaseService.SendFeatureData(dataSendFireBase, model);
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
                _logger.LogInformation(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Create Feature Notification: " + ex.Message);
                response.Status = 100;
                response.Data = false;
                response.Message = Constant.MSG_ERROR;
            }

            return response;
        }

        /// <summary>
        /// LVTan
        /// Create Feature Notification
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<JsonResponseModel> Create(FeatureNotificationModel model)
        {
            JsonResponseModel response = new JsonResponseModel();

            try
            {
                _logger.LogInformation("Create Feature Notification");
                FeatureNotificationService _service = new FeatureNotificationService(_config);
                var data = await _service.Create(model,User, Request.Headers["Authorization"].ToString());
                if (data)
                {
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
                _logger.LogInformation(JsonConvert.SerializeObject(response));
            }
            catch(Exception ex)
            {
                _logger.LogError("Create Feature Notification: " + ex.Message);
                response.Status = 100;
                response.Data = false;
                response.Message = Constant.MSG_ERROR;
            }

            return response;
        }

        /// <summary>
        /// LVTan
        /// Update Feature Notification
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("update")]
        public JsonResponseModel Update(FeatureNotificationModel model)
        {
            JsonResponseModel response = new JsonResponseModel();

            try
            {
                _logger.LogInformation("Update Feature Notification");
                FeatureNotificationService _service = new FeatureNotificationService(_config);
                bool result = _service.Update(model, User);
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
                _logger.LogInformation(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Update Feature Notification: " + ex.Message);
                response.Status = 100;
                response.Data = false;
                response.Message = Constant.MSG_UPDATE_FALSE;
            }

            return response;
        }

        /// <summary>
        /// LVTan
        /// Delete Feature Notification
        /// </summary>
        /// <param name="curUserId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public JsonResponseModel Delete(string curUserId, long id)
        {
            JsonResponseModel response = new JsonResponseModel();

            try
            {
                _logger.LogInformation("Delete Feature Notification with id : " + id);                
                FeatureNotificationService _service = new FeatureNotificationService(_config);
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
                _logger.LogInformation(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete Feature Notification: " + ex.Message);
                response.Status = 100;
                response.Data = false;
                response.Message = Constant.MSG_UPDATE_FALSE;
            }

            return response;
        }
    }
}
