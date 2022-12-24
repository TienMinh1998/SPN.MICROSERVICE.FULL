using AutoMapper;
using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Models;
using Hola.Api.Requests;
using Hola.Api.Service;
using Hola.Api.Service.BaseServices;
using Hola.Core.Model;
using Hola.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
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
        public async Task<JsonResponseModel> GetQuestionById([FromBody] QuestionModelStandard request)
        {
            try
            {
                bool condition = false;
                if (request.IsDesc==null || request.IsDesc==false)
                {
                    condition = false;
                }
                else
                {
                    condition = true;
                }

                Func<QuestionStandard, bool> lastCondition = m => true;
                var question = _questionStandardService.GetListPaged(request.PageNumber, request.PageSize, lastCondition,request.columnname,condition);
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
        public async Task<JsonResponseModel> GetAllQuestionByTopic([FromBody] GetStandQuestionRequest request)
        {
            try
            {
                string query = "SELECT  a.\"English\", a.\"Phonetic\" , a.\"MeaningEnglish\",  a.\"MeaningVietNam\"   FROM  (public.\"QuestionStandards\" q " +
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
        /// <summary>
        /// Thêm từ vào topic
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("AddQuestionToTopic")]
        public async Task<JsonResponseModel> AddQuestionToTopic ([FromBody] AddQuestionToTopic1 model)
        {
            try
            {
                string sql_Add = $"INSERT INTO usr.\"QuestionStandardDetail\" (\"QuestionID\", \"TopicID\") VALUES({model.QuestionID}, {model.TopicID});";
                var response = _dapper.Execute(sql_Add);
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
    }
}

