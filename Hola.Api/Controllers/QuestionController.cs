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


        [HttpGet("GetCategories")]
        public async Task<JsonResponseModel> GetAllCategory()
        {
            var result = await qesQuestionService.GetAllCategory();
            return JsonResponseModel.Success(result);
        }

        [HttpGet("GetQuestion/{ID}")]
        public async Task<JsonResponseModel> GetQuestionById(int ID)
        {
            var result = await qesQuestionService.GetListQuestionByCategoryId(ID);
            return JsonResponseModel.Success(result);
        }
    }
}
