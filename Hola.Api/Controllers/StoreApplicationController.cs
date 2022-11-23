using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using Hola.Api.Models;
using Hola.Api.Service;
using Hola.Core.Common;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hola.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class StoreApplicationController : ControllerBase
    {
        private readonly ILogger<StoreApplicationController> _logger;
        private readonly IOptions<SettingModel> _config;

        public StoreApplicationController(ILogger<StoreApplicationController> logger, IOptions<SettingModel> config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet("status/historybyuser")]
        public JsonResponseModel GetApplyStatusHistoryByUser()
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Store Status History By User");
                StoreApllicationService _service = new StoreApllicationService(_config);
                var userId = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
                var data = _service.GetApplyStatusHistoryByUser(userId);
                if (data.Any())
                {
                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;
                    response.Data = data;
                }
                else
                {
                    response.Status = 201;
                    response.Message = ErrorCodes.NullData;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Store Status History By User:" + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_DATA_NOT_FOUND;
            }
            return response;

        }
    }
}
