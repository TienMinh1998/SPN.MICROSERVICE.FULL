using Hola.Api.Service;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    public class QuestionController : ControllerBase
    {
        private readonly ILogger<AgencyManagementController> _logger;
        private readonly IOptions<SettingModel> _config;
        private readonly QuestionService qesQuestionService;

        public QuestionController(ILogger<AgencyManagementController> logger, IOptions<SettingModel> config, QuestionService qesQuestionService)
        {
            _logger = logger;
            _config = config;
            this.qesQuestionService = qesQuestionService;
        }


        [HttpPost("GetCategories")]
        public async Task<JsonResponseModel> Search()
        {
            var result = await qesQuestionService.GetAllQuestion();
            return JsonResponseModel.Success(result);
        }
    }
}
