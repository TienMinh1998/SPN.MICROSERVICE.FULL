using Hola.Api.Service;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Hola.Api.Models.Questions;
using Microsoft.AspNetCore.Authorization;

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


    

        [HttpGet("GetQuestion/{ID}")]
        public async Task<JsonResponseModel> GetQuestionById(int ID)
        {
            var result = await qesQuestionService.GetListQuestionByCategoryId(ID,0);
            return JsonResponseModel.Success(result);
        }

        [HttpGet("GetQuestionDeleted/{ID}")]
        public async Task<JsonResponseModel> GetQuestionDeletedById(int ID)
        {
            var result = await qesQuestionService.GetListQuestionByCategoryId(ID,1);
            return JsonResponseModel.Success(result);
        }

        [HttpPost("AddQuestion")]
        public async Task<JsonResponseModel> AddQuestion([FromBody] QuestionAddModel model)
        {
            var result = await qesQuestionService.AddQuestion(model);
            return JsonResponseModel.Success(result);
        }


        [HttpPost("DeleteQuestion")]
        public async Task<JsonResponseModel> AddQuestion([FromBody] DeleteQuestionRequest request)
        {
            var result = await qesQuestionService.DeleteQuestion(request.ID);
            return JsonResponseModel.Success(result);
        }

        [HttpGet("CountQuestion")]
        public async Task<JsonResponseModel> CountQuestion()
        {
            var result = await qesQuestionService.CountQuestion();
            return JsonResponseModel.Success(result);
        }
    }
}
