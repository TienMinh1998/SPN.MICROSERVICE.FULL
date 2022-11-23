using Hola.Api.Models;
using Hola.Api.Service;
using Hola.Core.Common;
using Hola.Core.Model;
using Hola.Core.Model.CommonModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;

namespace Hola.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class AnnouncementCategoryController : ControllerBase
    {
        // GET
        private readonly ILogger<AnnouncementCategoryController> _logger;

        private readonly IOptions<SettingModel> _config;

        public AnnouncementCategoryController(ILogger<AnnouncementCategoryController> logger, IOptions<SettingModel> config)
        {
            _logger = logger;
            _config = config;
        }

        /// <summary>
        /// LVTan
        /// Search Announcement Category
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [HttpPost("search")]
        public JsonResponseModel Search(AnnouncementCategoryFilterModel filters)
        {
            JsonResponseModel response = new JsonResponseModel();

            try
            {
                _logger.LogInformation("Search Announcement Category");
                AnnouncementCategoryService _service = new AnnouncementCategoryService(_config);
                var data = _service.Search(filters);
                if (data != null)
                {
                    response.Status = 200;
                    response.Data = data;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Data = new DataResultsModel();
                    response.Message = Constant.MSG_DATA_NOT_FOUND;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Search Announcement Category: " + ex.Message);
                response.Status = 100;
                response.Data = new DataResultsModel();
                response.Message = Constant.MSG_DATA_NOT_FOUND;
            }

            return response;
        }

        /// <summary>
        /// LVTan
        /// Create Announcement Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public JsonResponseModel create(AnnouncementCategoryModel category)
        {
            JsonResponseModel response = new JsonResponseModel();

            try
            {
                _logger.LogInformation("Create Announcement Category");
                AnnouncementCategoryService _service = new AnnouncementCategoryService(_config);
                var data = _service.Create(category,User);
                if (data)
                {
                    response.Status = 200;
                    response.Data = true;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Data = false;
                    response.Message = Constant.MSG_ERROR;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Create Announcement Category: " + ex.Message);
                response.Status = 100;
                response.Data = false;
                response.Message = Constant.MSG_ERROR;
            }

            return response;
        }

        /// <summary>
        /// LVTan
        /// Update Announcement Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost("update")]
        public JsonResponseModel Update(AnnouncementCategoryModel category)
        {
            JsonResponseModel response = new JsonResponseModel();

            try
            {
                _logger.LogInformation("Update Announcement Category");
                AnnouncementCategoryService _service = new AnnouncementCategoryService(_config);
                bool result = _service.Update(category);
                if (result)
                {
                    response.Status = 200;
                    response.Data = true;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Data = false;
                    response.Message = Constant.MSG_UPDATE_FALSE;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Update Announcement Category: " + ex.Message);
                response.Status = 100;
                response.Data = false;
                response.Message = Constant.MSG_UPDATE_FALSE;
            }

            return response;
        }

        /// <summary>
        /// LVTan
        /// Delete Announcement Category
        /// </summary>
        /// <param name="curUserId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpPost("delete")]
        public JsonResponseModel Delete(long categoryId)
        {
            JsonResponseModel response = new JsonResponseModel();

            try
            {
                _logger.LogInformation("Delete Announcement Category with id : " + categoryId);
                AnnouncementCategoryService _service = new AnnouncementCategoryService(_config);
                var result = _service.Delete(categoryId);
                if (result)
                {
                    response.Status = 200;
                    response.Data = result;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Data = false;
                    response.Message = Constant.MSG_UPDATE_FALSE;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete Announcement Category: " + ex.Message);
                response.Status = 100;
                response.Data = false;
                response.Message = Constant.MSG_UPDATE_FALSE;
            }

            return response;
        }
    }
}