using AutoMapper;
using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Models;
using Hola.Api.Service.CoursServices;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    public class CoursController : ControllerBase
    {
        private readonly ICoursService _coursService;
        private readonly IMapper _mapper;
        public CoursController(ICoursService coursService, IMapper mapper)
        {
            this._coursService = coursService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lấy ra tất cả các khóa học hiện tại đang có
        /// </summary>
        /// <returns></returns>
        [HttpGet("Cours")]
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
        /// Thêm 1 khóa học mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Course")]
        public async Task<JsonResponseModel> AddCourse([FromBody] CourseModel model)
        {
            try
            {
                var requestModel = _mapper.Map<Cours>(model);
                requestModel.created_on = DateTime.UtcNow;
                var response =  await _coursService.AddAsync(requestModel);
                return JsonResponseModel.Success(response);
            }
            catch (System.Exception ex)
            {

                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }
    }
}
