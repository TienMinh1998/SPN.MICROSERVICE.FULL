using AutoMapper;
using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Models;
using Hola.Api.Service;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    public class TopicController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TopicController> _logger;
        private readonly ITopicService _topicService;
        public TopicController(IMapper mapper,
            ILogger<TopicController> logger,
            ITopicService topicService = null)
        {
            _mapper = mapper;
            _logger = logger;
            _topicService = topicService;
        }

        /// <summary>
        /// Lấy ra tất cả các chủ đề theo khóa học
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetTopicByCoursId")]
        public async Task<JsonResponseModel> Topics([FromBody] GetTopicModel model)
        {
            try
            {
                var response = await _topicService.GetAllAsync(x=>x.FK_Course_Id==model.CoursId);
                return JsonResponseModel.Success(response);

            }
            catch (System.Exception ex)
            {

                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        [HttpGet("GetTopicById/{ID}")]
        public async Task<JsonResponseModel> Topic(int ID)
        {
            try
            {
                var response = await _topicService.GetFirstOrDefaultAsync(x=>x.PK_Topic_Id==ID);
                if (response!=null)
                {
                    return JsonResponseModel.Success(response);
                }
                return JsonResponseModel.Success(new List<string>(),$"Không tìm thấy Topic có Id= '{ID}'");
            }
            catch (System.Exception ex)
            {

                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        [HttpPost("AddTopic")]
        public async Task<JsonResponseModel> AddTopic([FromBody] AddTopicModel model)
        {
            try
            {
                var topicEntity = _mapper.Map<Topic>(model);
                topicEntity.created_on = DateTime.UtcNow;
                var response =await _topicService.AddAsync(topicEntity);
                return JsonResponseModel.Success(response);

            }
            catch (System.Exception ex)
            {

                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }
    }
}
