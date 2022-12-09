using Hola.Api.Service;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Hola.Api.Models.Questions;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Hola.Api.Common;
using System;
using System.Collections.Generic;

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


       
        /// <summary>
        /// Get Question By CategoryID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetQuestion/{ID}")]
        public async Task<JsonResponseModel> GetQuestionById(int ID)
        {
            var result = await qesQuestionService.GetListQuestionByCategoryId(ID,0);
            return JsonResponseModel.Success(result);
        }


        /// <summary>
        /// Get Delete Question
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetQuestionDeleted/{ID}")]
        public async Task<JsonResponseModel> GetQuestionDeletedById(int ID)
        {
            var result = await qesQuestionService.GetListQuestionByCategoryId(ID,1);
            return JsonResponseModel.Success(result);
        }


        /// <summary>
        /// Add new Question
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("AddQuestion")]
        [Authorize]
        public async Task<JsonResponseModel> AddQuestion([FromBody] QuestionAddModel model)
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == SystemParam.CLAIM_USER).Value;
            model.fk_userid= int.Parse(userid);
            var result = await qesQuestionService.AddQuestion(model);
            return JsonResponseModel.Success(result);
        }
        /// <summary>
        /// Delete Question
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("DeleteQuestion")]
        public async Task<JsonResponseModel> DeleteQuestion([FromBody] DeleteQuestionRequest request)
        {
            var result = await qesQuestionService.DeleteQuestion(request.ID);
            return JsonResponseModel.Success(result);
        }
        /// <summary>
        /// Total Question and Total QuestionToday
        /// </summary>
        /// <returns></returns>
        [HttpPost("CountQuestion")]
        [Authorize]
        public async Task<JsonResponseModel> CountQuestion()
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == SystemParam.CLAIM_USER).Value;
            var totalToday = await qesQuestionService.CountQuestionToday(int.Parse(userid));
            var total = await qesQuestionService.CountQuestion();
            var result = new Dictionary<string, int>();
            result.Add("today", totalToday);
            result.Add("total", total);
            return JsonResponseModel.Success(result);
        }

      public class ToDayRequest
        {
           public DateTime today { get; set; }
        }
    }
}
