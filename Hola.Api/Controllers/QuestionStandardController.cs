using AutoMapper;
using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Requests;
using Hola.Api.Service;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
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
        public QuestionStandardController(IQuestionStandardService questionStandardService, 
            IMapper mapper)
        {
            _questionStandardService = questionStandardService;
            _mapper = mapper;
        }
        /// <summary>
        /// Lấy tất cả câu hỏi
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("AllQuestion")]
        public async Task<JsonResponseModel> GetQuestionById([FromBody] QuestionModelStandard request)
        {
            try
            {
                Func<QuestionStandard, bool> lastCondition = m => true;
                var question = _questionStandardService.GetListPaged(request.PageNumber, request.PageSize, lastCondition,request.columnname);
                return JsonResponseModel.Success(question);
            }
            catch (Exception ex)
            {

                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }
           
        }

        /// <summary>
        /// Thêm một từ tiêu chuẩn
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
    }
}
