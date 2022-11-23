using FirebaseAdmin.Messaging;
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Hola.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1")]
    public class FireBaseController : ControllerBase
    {
        private readonly ILogger<FireBaseController> _logger;
        private readonly IOptions<SettingModel> _config;

        public FireBaseController(ILogger<FireBaseController> logger, IOptions<SettingModel> config)
        {
            _logger = logger;
            _config = config;
        }



        /// <summary>
        /// nthai3
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("sendData")]
        [AllowAnonymous]
        public JsonResponseModel SendData(SendFireBaseDataRequest request)
        {
            var response = new JsonResponseModel();
            FireBaseService _service = new FireBaseService(_config);

            var respNoteCount = PingService.NoteCounts(request.RecipientIds);


            for (int i = 0; i < request.RecipientIds.Count; i++)
            {
                var recipentId = request.RecipientIds[i];
                //Push notification to firebase
                string token = _service.getToken(recipentId);
                var msg = new Message
                {

                    Apns = new ApnsConfig { Aps = new Aps { Sound = "default", Badge = respNoteCount[i] } },

                    Token = token,
                    Data = new Dictionary<string, string>()
                    {
                        { "event", request.Event},
                        { "dto", request.Data},
                    },
                };

                _service.SendFirebase(msg);
            }

            return response;
        }

        [HttpPost("SendForceLogoutNoti")]
        [AllowAnonymous]
        public async Task<JsonResponseModel> SendForceLogoutNoti(string recipientId)
        {
            var response = new JsonResponseModel();
            FireBaseService _service = new FireBaseService(_config);

            await _service.SendForceLogoutNoti(recipientId);

            return response;
        }

        [HttpPost("SendForceLogoutPinVerifyNoti")]
        [AllowAnonymous]
        public async Task<JsonResponseModel> SendForceLogoutPinVerifyNoti(string recipientId)
        {
            var response = new JsonResponseModel();
            FireBaseService _service = new FireBaseService(_config);

            await _service.SendForceLogoutPinVerifyNoti(recipientId);

            return response;
        }


        [HttpPost("sendCrypto")]
        [AllowAnonymous]
        public async Task<JsonResponseModel> sendCrypto(SendFireBaseDataRequest request)
        {
            var response = new JsonResponseModel();
            FireBaseService _service = new FireBaseService(_config);

            await _service.SendEventSendCrypto(request);

            return response;
        }

        [HttpPost("reciverCrypto")]
        [AllowAnonymous]
        public async Task<JsonResponseModel> reciverCrypto(SendFireBaseDataRequest request)
        {
            var response = new JsonResponseModel();
            FireBaseService _service = new FireBaseService(_config);

            await _service.ReciverCrypto(request);

            return response;
        }



        /// <summary>
        /// nthai3
        /// </summary>
        /// <param name="tokenFirebase"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public JsonResponseModel Update(FirebaseRequestModel tokenFirebase)
        {
            JsonResponseModel response = new JsonResponseModel();

            try
            {
                _logger.LogInformation("update Firebase token");
                FireBaseService _service = new FireBaseService(_config);
                var data = _service.updateTokenFireBase(tokenFirebase.FireBaseToken, User);
                if (data)
                {
                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Message = Constant.MSG_ERROR;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Search Announcement Category: " + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_ERROR;
            }

            return response;
        }

    }
}
