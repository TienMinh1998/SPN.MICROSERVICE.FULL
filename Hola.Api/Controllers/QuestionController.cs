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
      
        private readonly IOptions<SettingModel> _config;
        private readonly QuestionService qesQuestionService;

        public QuestionController( IOptions<SettingModel> config, QuestionService qesQuestionService)
        {
           
            _config = config;
            this.qesQuestionService = qesQuestionService;
        }


        [HttpPost("GetCategories")]
        public async Task<JsonResponseModel> Search()
        {
            var result = await qesQuestionService.GetAllCategory();
            return JsonResponseModel.Success(result);
        }

        [HttpPost("GetQuestion")]
        public async Task<JsonResponseModel> GetQuestionById(int categoryid)
        {
            var result = await qesQuestionService.GetListQuestionByCategoryId(categoryid);
            return JsonResponseModel.Success(result);
        }
    }
}
