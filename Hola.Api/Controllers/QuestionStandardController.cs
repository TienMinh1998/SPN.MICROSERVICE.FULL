using AutoMapper;
using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Models;
using Hola.Api.Requests;
using Hola.Api.Service;
using Hola.Api.Service.BaseServices;
using Hola.Core.Model;
using Hola.Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    [Route("QuestionStandard")]
    public class QuestionStandardController : ControllerBase
    {
        private IQuestionStandardService _questionStandardService;
        private readonly IMapper _mapper;
        private readonly DapperBaseService _dapper;
        public QuestionStandardController(IQuestionStandardService questionStandardService,
            IMapper mapper,
            DapperBaseService dapper)
        {
            _questionStandardService = questionStandardService;
            _mapper = mapper;
            _dapper = dapper;
        }

        /// <summary>
        /// Lấy về câu hỏi theo ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetQuestionById/{Id}")]
        [Authorize]
        public async Task<JsonResponseModel> GetQuestionById(int Id)
        {
            try
            {
                var response = await _questionStandardService.GetFirstOrDefaultAsync(x=>x.Pk_QuestionStandard_Id== Id);
                if (response!=null)
                {
                    return JsonResponseModel.Success(response, $"Lấy về từ có Id = {Id} thành công!");
                }
                else
                {
                    return JsonResponseModel.Error("Có lỗi trong quá trình lấy câu hỏi", 400);
                }
            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }
        /// <summary>
        /// Lấy ra tất cả các từ, column nhập vào tên trường muốn sắp xếp
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("AllQuestion")]
        [Authorize]
        public async Task<JsonResponseModel> Search([FromBody] QuestionModelStandard request)
        {
            try
            {
                bool condition = false;
                Func<QuestionStandard, bool> searchCondition = x =>
                ((string.IsNullOrEmpty(request.searchKey) || request.searchKey == "*"
                || x.English.Contains(request.searchKey, StringComparison.OrdinalIgnoreCase)
                || x.MeaningVietNam.Contains(request.searchKey, StringComparison.OrdinalIgnoreCase)) && 
                  string.IsNullOrEmpty(request.Date)?true:x.created_on.ToString("yyyy-MM-dd")==request.Date);

                if (request.IsDesc==null || request.IsDesc==false)
                {
                    condition = false;
                }
                else
                {
                    condition = true;
                }
                var question = _questionStandardService.GetListPaged(request.PageNumber, request.PageSize, searchCondition, request.columnname,condition);
                question.currentPage = request.PageNumber;
                return JsonResponseModel.Success(question);
            }
            catch (Exception ex)
            {

                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }
           
        }

        /// <summary>
        /// Thêm một từ mới 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("AddStandardQuestion")]
        [Authorize]
        public async Task<JsonResponseModel> Add([FromBody] AddQuestionStandardModel request)
        {
            try
            {
                var command = _mapper.Map<QuestionStandard>(request);
                command.created_on = DateTime.UtcNow;
                command.IsDeleted = false;
                var checkquestion = await _questionStandardService.GetFirstOrDefaultAsync(x=>x.English== request.English);
                if (checkquestion == null)
                {
                    var respoinse = await _questionStandardService.AddAsync(command);
                    return JsonResponseModel.Success(respoinse);
                }

                return JsonResponseModel.SERVER_ERROR($"{request.English} đã tồn tại rồi");
               
            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }
        /// <summary>
        /// Lấy ra các từ theo chủ đề
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Get_StandQuesByTopic")]
        [Authorize]
        public async Task<JsonResponseModel> GetAllQuestionByTopic([FromBody] GetStandQuestionRequest request)
        {
            try
            {
                string query = "SELECT a.\"Pk_QuestionStandard_Id\",  a.\"English\", a.\"Phonetic\" , a.\"MeaningEnglish\",  a.\"MeaningVietNam\"   FROM  (public.\"QuestionStandards\" q " +
                    "\r\ninner join usr.\"QuestionStandardDetail\" qd on q.\"Pk_QuestionStandard_Id\"" +
                    $" = qd.\"QuestionID\" ) a\r\ninner join usr.topic tq on tq.\"PK_Topic_Id\" = a.\"TopicID\"\r\nwhere a.\"TopicID\" = {request.TargetID}";

                var response = await _dapper.GetAllAsync<QuestionStandardModel>(query.AddPadding(request.pageNumber, request.PageSize));
                return JsonResponseModel.Success(response);

            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        [HttpPost("Get_StandQuesByTopic/app")]
        [Authorize]
        public async Task<JsonResponseModel> GetAllQuestionByTopic_app([FromBody] GetStandQuestionRequest request)
        {
            try
            {
                int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                string query = "SELECT a.\"Pk_QuestionStandard_Id\",  a.\"English\", a.\"Phonetic\" , a.\"MeaningEnglish\", " +
                    " a.\"MeaningVietNam\", \r\ncase WHEN usq.\"StandardQuestion\" > 0 then true else false END as \"Tick\"\r\nFROM " +
                    " ((public.\"QuestionStandards\" q \r\ninner join usr.\"QuestionStandardDetail\" qd on q.\"Pk_QuestionStandard_Id\" " +
                    "= qd.\"QuestionID\" ) a inner join usr.topic tq on tq.\"PK_Topic_Id\" = a.\"TopicID\") \r\nleft join usr.\"UserStandardQuestion\" " +
                    $"usq on (a.\"Pk_QuestionStandard_Id\"  = usq.\"StandardQuestion\" and usq.\"UserId\" ={userid}) where a.\"TopicID\" = {request.TargetID} order by a.\"created_on\" asc";

                var response = await _dapper.GetAllAsync<QuestionStandardModel>(query.AddPadding(request.pageNumber, request.PageSize));
                return JsonResponseModel.Success(response);

            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        /// <summary>
        /// Thêm từ vào topic
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("AddQuestionToTopic")]
        [Authorize]
        public async Task<JsonResponseModel> AddQuestionToTopic ([FromBody] AddQuestionToTopic1 model)
        {
            try
            {
                // Thêm câu hỏi vào topic
                if (string.IsNullOrEmpty(model.QuestionID.ToString()) || string.IsNullOrEmpty(model.TopicID.ToString()))
                    return JsonResponseModel.Success("Sai định dạng dữ liệu đầu vào!");
                string sql_Add = $"INSERT INTO usr.\"QuestionStandardDetail\" (\"QuestionID\", \"TopicID\") VALUES({model.QuestionID}, {model.TopicID});";
                var response = _dapper.Execute(sql_Add);
                // Cập nhật lại trường đã thêm vào topic bằng true 
                string sqlUpdate = $"UPDATE public.\"QuestionStandards\" SET \"Added\"=true WHERE \"Pk_QuestionStandard_Id\"={model.QuestionID}";
                var responseUpdatge = _dapper.Execute(sqlUpdate);

                return JsonResponseModel.Success("Thêm thành công");
            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        /// <summary>
        /// Cập nhật câu hỏi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("UpdateQuestionStandard")]
        [Authorize]
        public async Task<JsonResponseModel> Update([FromBody] UpdateQuestionStandardModel model)
        {
            try
            {

                string count = $"SELECT COUNT(1) FROM public.\"QuestionStandards\" where \"Pk_QuestionStandard_Id\" = {model.Id}";
                var count_Response = _dapper.QueryFirstOrDefault<int>(count);
                if (count_Response != 1)
                    return JsonResponseModel.Error("Câu hỏi không tồn tại, vui lòng thử lại", 400);
                string sql_update = $"UPDATE public.\"QuestionStandards\"\r\nSET \"English\"='{model.English}', \"Phonetic\"='{model.Phonetic}', \"MeaningEnglish\"='{model.MeaningEnglish}'," +
                    $" \"MeaningVietNam\"='{model.MeaningVietNam}', \"Note\"='{model.Note}'\r\nWHERE \"Pk_QuestionStandard_Id\"={model.Id};";
                var response = _dapper.Execute(sql_update);
                return JsonResponseModel.Success("Cập nhật thành công");
            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }
        /// <summary>
        /// Xóa câu hỏi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("QuestionStandard/{id}")]
        public async Task<JsonResponseModel> DeleteQuestion(int id)
        {
            try
            {
                var question = await _questionStandardService.GetFirstOrDefaultAsync(x => x.Pk_QuestionStandard_Id == id);
                if (question == null)
                    return JsonResponseModel.Error($"Từ mới Id='{id}' không tồn tại", 400);
                await _questionStandardService.DeleteAsync(question);
                return JsonResponseModel.Success(new List<string>(), $"Xóa thành công từ mới Id ='{id}'");
            }
            catch (Exception ex)
            {

                return JsonResponseModel.Error(ex.Message, 500);
            }

        }


        [HttpGet("History")]
        [Authorize]
        public async Task<JsonResponseModel> GetHistory()
        {
            try
            {
                int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                string query = "SELECT id, fk_userid, count_word, percent_day, note, created_on " +
                    $"FROM history.userhistory where fk_userid ={userid} order by created_on desc ";
                var response = await _dapper.GetAllAsync<HistoryModel>(query);
                return JsonResponseModel.Success(response);

            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        [HttpPost("Tick")]
        [Authorize]
        public async Task<JsonResponseModel> TickQuestion([FromBody] TickQuestionRequest request)
        {
            try
            {
                int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                string sql = $"INSERT INTO usr.\"UserStandardQuestion\" (\"StandardQuestion\", \"UserId\") VALUES({request.QuestionStandardId}, {userid});";
                await _dapper.Execute(sql);
                return JsonResponseModel.Success("Update thành công");
            }
            catch (Exception)
            {
                return JsonResponseModel.Error("Thêm thất bại. Từ đã tồn tại", 400);
                
            }
        }
    }
}

