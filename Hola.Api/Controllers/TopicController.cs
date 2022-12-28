using AutoMapper;
using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Models;
using Hola.Api.Service;
using Hola.Core.Model;
using Hola.GoogleCloudStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    [Authorize]
    public class TopicController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TopicController> _logger;
        private readonly ITopicService _topicService;
        public readonly IUploadFileGoogleCloudStorage _GoogleCloudStorage;
        public TopicController(IMapper mapper,
            ILogger<TopicController> logger,
            ITopicService topicService = null,
            IUploadFileGoogleCloudStorage googleCloudStorage = null)
        {
            _mapper = mapper;
            _logger = logger;
            _topicService = topicService;
            _GoogleCloudStorage = googleCloudStorage;
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
                var response = await _topicService.GetAllAsync(x => x.FK_Course_Id == model.CoursId);
                return JsonResponseModel.Success(response);

            }
            catch (System.Exception ex)
            {

                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }
        /// <summary>
        /// Lấy ra topic theo ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetTopicById/{ID}")]
        public async Task<JsonResponseModel> Topic(int ID)
        {
            try
            {
                var response = await _topicService.GetFirstOrDefaultAsync(x => x.PK_Topic_Id == ID);
                if (response != null)
                {
                    return JsonResponseModel.Success(response);
                }
                return JsonResponseModel.Success(new List<string>(), $"Không tìm thấy Topic có Id= '{ID}'");
            }
            catch (System.Exception ex)
            {

                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        /// <summary>
        ///  Thêm mới 1 topic
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("AddTopic")]
        public async Task<JsonResponseModel> AddTopic([FromBody] AddTopicModel model)
        {
            try
            {
                var topicEntity = _mapper.Map<Topic>(model);
                topicEntity.created_on = DateTime.UtcNow;


                var response = await _topicService.AddAsync(topicEntity);
                return JsonResponseModel.Success(response);

            }
            catch (System.Exception ex)
            {

                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }
        /// <summary>
        /// Cập nhật chủ đề
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("UpdateTopic")]
        public async Task<JsonResponseModel> Update([FromForm] UpdateTopicModel model)
        {
            try
            {
                var resultUrl = string.Empty;
                var topic = await _topicService.GetFirstOrDefaultAsync(x => x.PK_Topic_Id == model.pK_Topic_Id);
                if (topic == null)
                    return JsonResponseModel.Error($" Chủ đề {model.pK_Topic_Id} không tồn tại ", 400);

                if (model.file!=null)
                {
                    var filename = DateTime.Now.ToString() + model.file.FileName;
                    var filePath = Path.GetTempFileName();
                    using (var stream = System.IO.File.Create(filePath))
                        await model.file.CopyToAsync(stream);
                    resultUrl = _GoogleCloudStorage.UploadFile(filePath, "5512421445", filename, "credentials.json", "image", "image/jpeg");
                }
                else
                {
                    resultUrl = topic.Image;
                }

                var entity = _mapper.Map<Topic>(model);
                entity.Image = resultUrl;
                entity.created_on = topic.created_on;
                var updateTopic = await _topicService.UpdateAsync(entity);
                if (updateTopic != null)
                    return JsonResponseModel.Success(updateTopic, "Cập nhật thông tin chủ đề thành công");
                return JsonResponseModel.Error("Server quá tải, vui lòng thử lại sau", 500);
            }
            catch (Exception ex)
            {

                return JsonResponseModel.Error(ex.Message, 500);
            }
        }
        /// <summary>
        /// Lấy ra danh sách topic có phân trang
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost("List-toppic")]
        public async Task<JsonResponseModel> GetAllPadding([FromBody] GetPadingRequest requestModel)
        {
            try
            {
                Func<Topic, bool> lastCondition;
                if (requestModel.courseId!=null)
                {
                    lastCondition = m => m.FK_Course_Id == requestModel.courseId.Value;
                }
                else
                {
                    lastCondition = m => true;
                }

              

                var toppics = _topicService.GetListPaged(requestModel.pageNumber, requestModel.pageSize, lastCondition, requestModel.columnSort, requestModel.isDesc);
                toppics.currentPage = requestModel.pageNumber;
                if (toppics != null)
                {
                    return JsonResponseModel.Success(toppics);
                }
                else
                {
                    return JsonResponseModel.Success(new List<string>(), "Danh sách rỗng");
                }
            }
            catch (System.Exception ex)
            {

                return JsonResponseModel.SERVER_ERROR();
            }

        }

        /// <summary>
        /// Xóa 1 topic
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("topic/{id}")]
        public async Task<JsonResponseModel> DeleteCourse(int id)
        {
            try
            {
                var topic = await _topicService.GetFirstOrDefaultAsync(x => x.PK_Topic_Id == id);
                if (topic == null)
                    return JsonResponseModel.Error($"Chủ đề Id='{id}' không tồn tại", 400);
                await _topicService.DeleteAsync(topic);
                return JsonResponseModel.Success(new List<string>(), $"Xóa thành công chủ đề Id ='{id}'");
            }
            catch (Exception ex)
            {
                return JsonResponseModel.Error(ex.Message, 500);
            }

        }

    }
}
