
#region Package
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Hola.Api.Models.Questions;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Hola.Api.Common;
using System;
using System.Collections.Generic;
using Hola.Api.Service.V1;
using Hola.Core.Helper;
using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Service.CateporyServices;
using Hola.Api.Models.Dic;
using Hola.Api.Requests;
using Hola.Api.Service.BaseServices;
#endregion


namespace Hola.Api.Controllers
{
    public class QuestionController : ControllerBase
    {
        #region Properties and Construtor
        private readonly IOptions<SettingModel> _config;
        private readonly Service.QuestionService qesQuestionService;
        private readonly IQuestionService _questionService;
        private readonly ICategoryService categoryService;
        private readonly DapperBaseService _dapper;

        public QuestionController(IOptions<SettingModel> config,
            Service.QuestionService qesQuestionService,
            IQuestionService qService, ICategoryService categoryService, DapperBaseService dapper)
        {

            _config = config;
            this.qesQuestionService = qesQuestionService;
            _questionService = qService;
            this.categoryService = categoryService;
            _dapper = dapper;
        }
        #endregion

        #region Add
        [HttpPost("AddQuestion")]
        [Authorize]
        public async Task<JsonResponseModel> AddQuestion([FromBody] QuestionAddModel model)
        {
            try
            {
                int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                // Check question is available 
                var question_available = await _questionService.GetFirstOrDefaultAsync(x => x.fk_userid == userid && x.questionname.ToLower() == model.QuestionName.ToLower());
                if (question_available == null)
                {
                    string audio = "";
                    string phonetic = "";
                    string desfinition = "";
                    string type = "";
                    try
                    {
                        // Get infomation from oxfordDictionary
                        APICrossHelper api = new APICrossHelper();
                        string word = model.QuestionName;
                        var response1 = await api.GetFromDictionary<ResultFromOxford>(word, "en-us");
                        var audioFile = response1.Results.FirstOrDefault()
                            .lexicalEntries.FirstOrDefault()
                            .entries.FirstOrDefault()
                            .pronunciations
                            .FirstOrDefault().audioFile;
                        // Get phoneticSpelling
                        var phoneticSpelling = response1.Results.FirstOrDefault()
                            .lexicalEntries.FirstOrDefault()
                            .entries.FirstOrDefault()
                            .pronunciations
                            .FirstOrDefault().phoneticSpelling;
                        // get definition
                        var def = response1.Results.FirstOrDefault()
                            .lexicalEntries.FirstOrDefault().entries.FirstOrDefault().senses.FirstOrDefault().definitions.FirstOrDefault();
                        // Get type Of word
                        type = response1.Results.FirstOrDefault().lexicalEntries.FirstOrDefault().lexicalCategory.text;
                        phonetic = $"[{phoneticSpelling}]";
                        audio = audioFile;
                        desfinition = def;
                    }
                    catch (Exception ex)
                    {
                    }

                    // Add question to repository
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
                        questionname = model.QuestionName,
                        definition = desfinition,
                        Type = type
                    };
                    await _questionService.AddAsync(question);
                    string sqlquery = "update usr.categories \r\nset totalquestion = (select count(1) from usr.question " +
                        $"where  category_id = {model.Category_Id} and fk_userid ={userid})\r\nwhere \"Id\" = {model.Category_Id} and fk_userid ={userid}\r\n";
                    await _dapper.Execute(sqlquery);
                    return JsonResponseModel.Success(question);
                }
                else
                {
                    return JsonResponseModel.Error("Question is Exsit", 400);
                }
            }
            catch (Exception)
            {
                return JsonResponseModel.Success(new Question());

            }
        }
        #endregion

        #region Get and query
        /// <summary>
        /// get question By category ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetQuestion/{ID}")]
        public async Task<JsonResponseModel> GetQuestionById(int ID)
        {
            try
            {
                string query = string.Format("SELECT * FROM usr.question where is_delete !=1 and category_id ={0} order by created_on desc", ID);
                var list_question = await _dapper.GetAllAsync<Question>(query);
                return JsonResponseModel.Success(list_question);
            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }
        }
        [HttpGet("GetQuestion")]
        [Authorize]
        public async Task<JsonResponseModel> GetQuestion()
        {
            try
            {
                var str_userid = User.Claims.FirstOrDefault(c => c.Type == SystemParam.CLAIM_USER).Value;
                int userid = int.Parse(str_userid);
                string query = string.Format("SELECT * FROM usr.question where is_delete !=1 and category_id in" +
                    " (SELECT \"Id\" FROM usr.categories) and fk_userid ={0} order by created_on desc", userid);

                var response = await _dapper.GetAllAsync<Question>(query);
                return JsonResponseModel.Success(response);
            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }
        /// <summary>
        /// Get Delete Question
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetQuestionDeleted/{ID}")]
        public async Task<JsonResponseModel> GetQuestionDeletedById(int ID)
        {
            var question = await _questionService.GetAllAsync(x => (x.category_id == ID) && (x.is_delete == 1));
            return JsonResponseModel.Success(question);
        }
        /// <summary>
        /// Lấy danh sách câu hỏi đã học, có phân trang
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("v2/GetQuestionDeleted")]
        public async Task<JsonResponseModel> GetLisLearnQuestion([FromBody] PaddingQuestionRequest model)
        {
            // Lấy ra danh sách đã học trong ngày hôm này
            var a = DateTime.Now.Day;
            Func<Question, bool> condition = x => (x.is_delete == 1 && x.category_id == model.Category_Id && x.created_on.Day == DateTime.UtcNow.Day);
            var question = _questionService.GetListPaged(model.PageNumber, model.PageSize, condition, model.SortColumn, model.IsDesc);
            return JsonResponseModel.Success(question);
        }
        /// <summary>
        /// Cập nhật thành đã học
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("DeleteQuestion")]
        public async Task<JsonResponseModel> DeleteQuestion([FromBody] DeleteQuestionRequest request)
        {
            var question = await _questionService.GetFirstOrDefaultAsync(x => x.id == request.ID);
            question.is_delete = 1;
            question.created_on = DateTime.Now;
            await _questionService.UpdateAsync(question);
            return JsonResponseModel.Success(true);
        }
        #endregion
        /// <summary>
        /// Lấy tất cả các câu hỏi của người dùng, có phân trang
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("GetListLearnedByUser")]
        [Authorize]
        public async Task<JsonResponseModel> GetListOfLeared([FromBody] WordPadingRequest model)
        {
            try
            {
                int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                Func<Question, bool> condition = x => (x.is_delete == 1 && x.fk_userid == userid)
                && (x.questionname.Contains(model.SearchKey));

                var question = _questionService.GetListPaged(model.PageNumber, model.PageSize, condition, model.SortColumn, model.IsDesc);
                return JsonResponseModel.Success(question);
            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        #region Extension
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
        #endregion

    }
}
