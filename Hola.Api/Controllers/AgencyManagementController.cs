


using Hola.Api.Common;
using Hola.Api.Models;
using Hola.Core.Common;
using Hola.Core.Model;
using Hola.Core.Model.CommonModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Hola.Api.Service;
using Sentry;

namespace Hola.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "v1")]
    public class AgencyManagementController : ControllerBase
    {
        private readonly ILogger<AgencyManagementController> _logger;
        private readonly IOptions<SettingModel> _config;
        public AgencyManagementController(IOptions<SettingModel> config, ILogger<AgencyManagementController> logger)
        {
            _config = config;
            _logger = logger;
        }
        /// <summary>
        /// nvmTien
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [HttpPost("Search")]
        public JsonResponseModel Search(AgentApplicationSearchRequest filters)
        {
            try
            {
                AgentApplicationService _service = new AgentApplicationService(_config);
                var data = _service.Search(filters);
                if (data == null) return JsonResponseModel.Response(100, new DataResultsModel(), Constant.MSG_DATA_NOT_FOUND);
                return JsonResponseModel.Response(Constant.SUCCESS_CODE, data, Constant.MSG_SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError("Search Announcement Category: " + ex.Message);
                return JsonResponseModel.Response(Constant.SERVER_ERROR_CODE, new DataResultsModel(), Constant.MSG_SERVER_ERROR);
            }
        }

        /// <summary>
        /// nvmTien
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPost("history")]
        public JsonResponseModel History(AgencyManagementHistoryRequest inputModel)
        {
            try
            {
                _logger.LogInformation("AgencyManagementController history");
                AgentApplicationService _service = new AgentApplicationService(_config);
                DataResultsModel data = _service.History(inputModel.UserID, inputModel.PageNumber, inputModel.PageLimit);
                if (data.TotalCount.Equals(0)) return JsonResponseModel.Response(SystemParam.ERRORCODE, new DataResultsModel(), Constant.MSG_DATA_NOT_FOUND);
                return JsonResponseModel.Response(SystemParam.SUCCESSCODE, data, Constant.MSG_SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error  " + ex.Message);
                return JsonResponseModel.Response(100, new DataResultsModel(), Constant.MSG_SERVER_ERROR);
            }
        }

        /// <summary>
        /// Criss
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("status/update")]
        public JsonResponseModel ReviewApplication(ReviewAgentApplicationRequest request)
        {
            try
            {
                _logger.LogInformation("AgencyManagermentController Start ");
                AgentApplicationService _service = new AgentApplicationService(_config);
                ApplyAgentService _applyAgentService = new ApplyAgentService(_config);
                string userid = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
                var result = _service.ReviewAgentApplication(request, userid);

                var usr = _applyAgentService.Get(request.Id);
                _service.UpdateStatus(new List<string> { usr.UserId }, Convert.ToInt16(request.StatusId));
                return JsonResponseModel.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("AgencyManagermentController :" + ex.ToString());
                Console.WriteLine(ex);
                return JsonResponseModel.Response(401, "", Constant.MSG_SERVER_ERROR);
            }

        }
    }
}
