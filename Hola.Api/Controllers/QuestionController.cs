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
using Hola.Api.Service.V1;
using EntitiesCommon.EntitiesModel;
using Hola.Core.Helper;
using Newtonsoft.Json;
using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Service.CateporyServices;

namespace Hola.Api.Controllers
{
    public class QuestionController : ControllerBase
    {

        private readonly IOptions<SettingModel> _config;
        private readonly Service.QuestionService qesQuestionService;
        private readonly IQuestionService _qService;
        private readonly ICategoryService categoryService;

        public QuestionController(IOptions<SettingModel> config,
            Service.QuestionService qesQuestionService,
            IQuestionService qService, ICategoryService categoryService)
        {

            _config = config;
            this.qesQuestionService = qesQuestionService;
            _qService = qService;
            this.categoryService = categoryService;
        }

        ///// <summary>
        ///// Get Question By CategoryID
        ///// </summary>
        ///// <param name="ID"></param>
        ///// <returns></returns>
        //[HttpGet("GetQuestion/{ID}")]
        //public async Task<JsonResponseModel> GetQuestionById(int ID)
        //{
        //    var result = await qesQuestionService.GetListQuestionByCategoryId(ID,0);
        //    return JsonResponseModel.Success(result);
        //}

        /// <summary>
        /// Test
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetQuestion/{ID}")]
        public async Task<JsonResponseModel> GetQuestionById(int ID)
        {
            var question = await _qService.GetAllAsync(x => (x.category_id == ID) && (x.is_delete != 1));

            return JsonResponseModel.Success(question);
        }

        /// <summary>
        /// Get Delete Question
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetQuestionDeleted/{ID}")]
        public async Task<JsonResponseModel> GetQuestionDeletedById(int ID)
        {
            var result = await qesQuestionService.GetListQuestionByCategoryId(ID, 1);
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
            // phiên âm của từ đó
            APICrossHelper api = new APICrossHelper();
            string word = model.QuestionName;
            var response = await api.Get<object>($"https://api.dictionaryapi.dev/api/v2/entries/en/{word}");
            var phienam = JsonConvert.DeserializeObject<List<ResponseDicModel>>(response.ToString());
            string phonetic = phienam.FirstOrDefault().phonetic;
            var audio = phienam.FirstOrDefault().phonetics.FirstOrDefault().audio;
            // Thêm Câu hỏi vào Kho từ 
            Question question = new Question()
            {
                is_delete = 0,
                answer = model.Answer,
                audio = audio,
                category_id = model.Category_Id,
                phonetic = phonetic,
                created_on = DateTime.Now,
                fk_userid = model.fk_userid,
                ImageSource = model.ImageSource,
                questionname = model.QuestionName +" "+ phonetic,
            };
            // Cập nhật lại trường đếm trong category
            var category = await categoryService.GetFirstOrDefaultAsync(x => x.Id == model.Category_Id);
            category.totalquestion += 1;
            category.priority += 1;

            await categoryService.UpdateAsync(category);
            await _qService.AddAsync(question);
            // Cập nhật lại trường đếm trong category
            return JsonResponseModel.Success(question);
        }
        /// <summary>
        /// Delete Question
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("DeleteQuestion")]
        public async Task<JsonResponseModel> DeleteQuestion([FromBody] DeleteQuestionRequest request)
        {
            var question = await _qService.GetFirstOrDefaultAsync(x => x.id == request.ID);
            question.is_delete = 1;
            await _qService.UpdateAsync(question);
            return JsonResponseModel.Success(true);
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
