using AutoMapper;
using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Models;
using Hola.Api.Service.CoursServices;
using Hola.Core.Model;
using Hola.GoogleCloudStorage;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    public class CoursController : ControllerBase
    {
        private readonly ICoursService _coursService;
        private readonly IMapper _mapper;
        public readonly IUploadFileGoogleCloudStorage _GoogleCloudStorage;
        public CoursController(ICoursService coursService, IMapper mapper, IUploadFileGoogleCloudStorage googleCloudStorage)
        {
            this._coursService = coursService;
            _mapper = mapper;
            _GoogleCloudStorage = googleCloudStorage;
        }

        /// <summary>
        /// Lấy thông tin khóa học theo ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("CoursById/{Id}")]
        public async Task<JsonResponseModel> Get_All_Cours(int Id)
        {
            try
            {
                var response = await _coursService.GetFirstOrDefaultAsync(x => x.Pk_coursId == Id);
                if (response == null)
                    return JsonResponseModel.Error($"Không tìm thấy khóa học có id '{Id}'", 404);
                return JsonResponseModel.Success(response, $"Lấy thông tin khóa học {response.Code} thành công.");

            }
            catch (System.Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        /// <summary>
        /// Lấy ra tất cả các khóa học hiện tại đang có
        /// </summary>
        /// <returns></returns>
        [HttpGet("Coursies")]
        public async Task<JsonResponseModel> Get_All_Cours()
        {
            try
            {
                var response = await _coursService.GetAllAsync();
                return JsonResponseModel.Success(response);

            }
            catch (System.Exception ex)
            {

                return JsonResponseModel.SERVER_ERROR();
            }

        }

        /// <summary>
        /// lấy ra tất cả các khóa học có phân trang
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost("List-Course")]
        public async Task<JsonResponseModel> GetAllPadding([FromBody] GetPadingRequest requestModel)
        {
            try
            {
                Func<Cours, bool> lastCondition = m => true;
                var questions = _coursService.GetListPaged(requestModel.pageNumber, requestModel.pageSize, lastCondition, requestModel.columnSort, requestModel.isDesc);
                questions.currentPage = requestModel.pageNumber;
                if (questions != null)
                {
                    return JsonResponseModel.Success(questions);
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
        /// Thêm 1 khóa học mới 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Course")]
        public async Task<JsonResponseModel> AddCourse([FromForm] CourseModelRequest model)
        {
            try
            {
                var requestModel = _mapper.Map<Cours>(model);
                var countResult = await _coursService.CountAsync(x => x.Code == model.code);
                if (countResult == 0)
                {
                    requestModel.created_on = DateTime.UtcNow;
                    try
                    {
                        var filename = model.file;
                        var filePath = Path.GetTempFileName();
                        using (var stream = System.IO.File.Create(filePath))
                            await model.file.CopyToAsync(stream);
                        var resultUrl = _GoogleCloudStorage.UploadFile(filePath, "5512421445", model.file.FileName, "credentials.json", "image", "image/jpeg");
                        requestModel.CoursImage = resultUrl;
                        var response = await _coursService.AddAsync(requestModel);
                        return JsonResponseModel.Success(response);
                    }
                    catch (Exception ex)
                    {
                        return JsonResponseModel.Error($"File image is Lagest {ex.Message}", 500);
                    }

                }
                else
                {
                    return JsonResponseModel.Error("Mã khóa học đã tồn tại", 400);
                }
            }
            catch (System.Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }

        /// <summary>
        /// Chỉnh xửa khóa học
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("Course")]
        public async Task<JsonResponseModel> EditCourse([FromForm] UpdateCourseModel model)
        {
            try
            {
                string resultUrl = string.Empty;
                var course = await _coursService.GetFirstOrDefaultAsync(x => x.Pk_coursId == model.pk_coursId);
                if (course == null)
                    return JsonResponseModel.Error("Khóa học không tồn tại", 400);
                if (model.file != null)
                {
                    var filename = DateTime.Now.ToString() + model.file.FileName;
                    var filePath = Path.GetTempFileName();
                    using (var stream = System.IO.File.Create(filePath))
                        await model.file.CopyToAsync(stream);
                    resultUrl = _GoogleCloudStorage.UploadFile(filePath, "5512421445", filename, "credentials.json", "image", "image/jpeg");
                }
                else
                {
                    resultUrl = course.CoursImage;
                }

                var entity = _mapper.Map<Cours>(model);
                entity.Code = course.Code;
                entity.created_on = course.created_on;
                entity.CoursImage = resultUrl;
                var updateCourse = await _coursService.UpdateAsync(entity);
                if (updateCourse != null)
                    return JsonResponseModel.Success(updateCourse, "Cập nhật thông tin khóa học thành công");
                return JsonResponseModel.Error("Server quá tải, vui lòng thử lại sau", 500);
            }
            catch (Exception ex)
            {

                return JsonResponseModel.Error(ex.Message, 500);
            }

        }

        /// <summary>
        /// Xóa 1 khóa học
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Course/{id}")]
        public async Task<JsonResponseModel> DeleteCourse(int id)
        {
            try
            {
                var course = await _coursService.GetFirstOrDefaultAsync(x => x.Pk_coursId == id);
                if (course == null)
                    return JsonResponseModel.Error($"Khóa học Id='{id}' không tồn tại", 400);
                await _coursService.DeleteAsync(course);
                return JsonResponseModel.Success(new List<string>(), $"Xóa thành công khóa học Id ='{id}'");
            }
            catch (Exception ex)
            {

                return JsonResponseModel.Error(ex.Message, 500);
            }

        }
    }
}
