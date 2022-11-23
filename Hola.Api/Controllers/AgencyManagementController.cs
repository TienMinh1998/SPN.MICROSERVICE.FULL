


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
        private readonly QuestionService qesQuestionService;
        public AgencyManagementController(IOptions<SettingModel> config, 
            ILogger<AgencyManagementController> logger, QuestionService qesQuestionService)
        {
            _config = config;
            _logger = logger;
            this.qesQuestionService = qesQuestionService;
        }
        /// <summary>
        /// nvmTien
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [HttpPost("GetCategories")]
        public async Task<JsonResponseModel> Search()
        {
            var result = await qesQuestionService.GetAllQuestion();
            return JsonResponseModel.Success(result);
        }

    }
}
