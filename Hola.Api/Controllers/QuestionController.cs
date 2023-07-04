
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
using System.Drawing;
using Hola.Core.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Quartz.Impl.Triggers;
using iText.StyledXmlParser.Jsoup.Nodes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        private readonly IWebHostEnvironment _hostEnvironment;

        public QuestionController(IOptions<SettingModel> config,
            Service.QuestionService qesQuestionService,
            IQuestionService qService, ICategoryService categoryService, DapperBaseService dapper, IWebHostEnvironment hostEnvironment)
        {

            _config = config;
            this.qesQuestionService = qesQuestionService;
            _questionService = qService;
            this.categoryService = categoryService;
            _dapper = dapper;
            _hostEnvironment = hostEnvironment;
        }
        #endregion

        #region Add
        [HttpPost("AddQuestion")]
        [Authorize]
        public async Task<JsonResponseModel> AddQuestion([FromBody] QuestionAddModel model)
        {
            // Phân quyền cho người dùng
            int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            string sql = "SELECT COUNT(*) FROM usr.\"User\" u \r\nINNER JOIN usr.\"UserRole\" ur ON u.\"Id\"  = ur.\"FK_UserID\"" +
                "  \r\nINNER JOIN usr.rolepermission  rp ON ur.\"FK_RoleID\"  = rp.\"FK_RoleID\" " +
                " \r\nINNER JOIN usr.\"permission\" p  ON rp.\"FK_PermissionID\"  " +
                $"= p.\"Id\" \r\nWHERE u.\"Id\" ={userid} and p.\"PermissionKey\"  = 'AddQuestion' and u.\"IsDeleted\" =0;";


            var hasPermission = _dapper.QueryFirstOrDefault<int>(sql);
            if (hasPermission == 0)
            {
                return JsonResponseModel.Error("Bạn chưa có quyền sử dụng chức năng này", 400);
            }

            var rootPath = _hostEnvironment.WebRootPath != null ? _hostEnvironment.WebRootPath : _hostEnvironment.ContentRootPath;
            string word = model.QuestionName;
            APICrossHelper api = new APICrossHelper();
            // Chạy bất đồng bộ để lấy về của nghĩa tiếng việt
            Task<CambridgeDictionaryModel> cambridgeDicTask = api.GetWord(word);
            Task<CambridgeDictionaryVietNamModel> vietnamMeaningTask = api.GetVietNamMeaning(word);


            await Task.WhenAll(cambridgeDicTask, vietnamMeaningTask);
            var cambridgeDicResponse = cambridgeDicTask.Result;
            var vietnamMeaning = vietnamMeaningTask.Result;


            string camAudio = cambridgeDicResponse?.Mp3;
            string camPhonetic = cambridgeDicResponse?.Phonetic;
            string camType = cambridgeDicResponse?.Type;
            string camDefinition = cambridgeDicResponse?.Definition;
            string camExample = cambridgeDicResponse?.Example;
            var oxfordWordSame = await api.GetSameType(word);

            try
            {
                var question_available = await _questionService.GetFirstOrDefaultAsync(x => x.fk_userid == userid && x.questionname.ToLower() == model.QuestionName.ToLower());
                List<string> sysnynoms = new List<string>();  // Từ đồng nghĩa
                string imageURL = "";
                string image = "";
                try
                {
                    // Get infomation from oxfordDictionary
                    if (string.IsNullOrEmpty(model.ImageSource)) // Nếu không truyền ảnh lên thì trả về 1 ảnh ví dụ.
                    {
                        var rImage = await api.IllustrationImage<RootObject>(word);
                        imageURL = rImage.hits.FirstOrDefault(x => !string.IsNullOrEmpty(x.webformatURL)).webformatURL;
                        image = await api.DownloadFileAsync(imageURL, "image", rootPath);
                    }
                }
                catch (Exception ex)
                {
                }

                string typeNote = "";
                typeNote = camType.Trim();
                if (camType.Trim().ToLower() == "adverb")
                {
                    typeNote = "adv";
                }
                else if (camType.Trim().ToLower() == "adjective")
                {
                    typeNote = "adj";
                }
                else if (camType.Trim().ToLower() == "noun")
                {
                    typeNote = "n";
                }
                else if (camType.Trim().ToLower() == "verb")
                {
                    typeNote = "v";
                }
                if (string.IsNullOrEmpty(model.Note))
                {
                    model.Note = "from dictionary.cambridge.org";
                }

                if (question_available == null)
                {
                    Question question = new Question()
                    {
                        is_delete = 0,
                        answer = $"({typeNote}) {string.Join(",", vietnamMeaning.Meaning)}",   // Xử lý chuỗi string
                        audio = camAudio,
                        category_id = model.Category_Id,
                        phonetic = $"/{camPhonetic}/",
                        created_on = DateTime.Now.AddHours(7),
                        fk_userid = model.fk_userid,
                        ImageSource = image,
                        questionname = model.QuestionName,  // Xử lý chuỗi string
                        definition = $"{camDefinition}",
                        Type = camType,
                        Synonym = string.Join(",", oxfordWordSame),
                        Note = model.Note                // ghi chú của người dùng nhập thêm
                    };
                    await _questionService.AddAsync(question);
                    string sqlquery = "update usr.categories \r\nset totalquestion = (select count(1) from usr.question " +
                        $"where  category_id = {model.Category_Id} and fk_userid ={userid})\r\nwhere \"Id\" = {model.Category_Id} and fk_userid ={userid}\r\n";

                    await _dapper.Execute(sqlquery);
                    return JsonResponseModel.Success(question);
                }
                else
                {
                    // không cần update image nữa vì nó có rồi!
                    question_available.is_delete = 0;
                    question_available.answer = $"({typeNote}) {string.Join(",", vietnamMeaning.Meaning)}";   // Xử lý chuỗi string
                    question_available.audio = camAudio;
                    question_available.category_id = model.Category_Id;
                    question_available.phonetic = $"/{camPhonetic}/";
                    question_available.created_on = DateTime.Now;
                    question_available.fk_userid = model.fk_userid;
                    question_available.questionname = model.QuestionName;  // Xử lý chuỗi string
                    question_available.definition = $"Nghĩa : {camDefinition}";
                    question_available.Type = camType;
                    question_available.Synonym = string.Join(",", oxfordWordSame);
                    question_available.Note = model.Note;                  // ghi chú của người dùng
                    question_available.Synonym = string.Join(",", oxfordWordSame);
                    await _questionService.UpdateAsync(question_available);
                    return JsonResponseModel.Success("Cập nhật thành công!");
                }
            }
            catch (Exception)
            {
                return JsonResponseModel.Error("Đây không phải từ tiếng anh, vui lòng kiểm tra lại", 400);

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
