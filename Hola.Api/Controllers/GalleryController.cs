using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Hola.Api.Model;
using Hola.Api.Models;
using Hola.Api.Service;
using Hola.Api.Service.UploadFile;
using Hola.Core.Common;
using Hola.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hola.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1")]
    public class GalleryController : ControllerBase
    {
        private readonly ILogger<GalleryController> _logger;
        private readonly IOptions<SettingModel> _config;
        private readonly IUploadFileService _uploadFileService;
        
        public GalleryController(ILogger<GalleryController> logger, IOptions<SettingModel> config, IUploadFileService uploadFileService)
        {
            _logger = logger;
            _config = config;
            _uploadFileService = uploadFileService;
        }

        /// <summary>
        /// John, Criss Edit 9/6/2022
        /// </summary>
        /// <param name="file1"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        public async  Task<JsonResponseModel> UploadImage(IFormFile file1)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Upload image");
                GalleryService _service = new GalleryService(_config);
                string error = string.Empty;
                var file = Request.Form.Files[0];
                var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                using var fileStream = file.OpenReadStream();
                var data =await _uploadFileService.UploadFile(User, filename, fileStream, out error);
                if (data != null)
                {
                    response.Status = 200;
                    response.Data = data;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Message = error;
                }

                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Upload Image: " + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_ERROR;
                return response;
            }
            return response;
        }

        /// <summary>
        /// LVTan,Criss Edit 9/6/2022
        /// </summary>
        /// <param name="file1"></param>
        /// <returns></returns>
        [HttpPost("UploadFile")]
        public async Task<JsonResponseModel> UploadFile(IFormFile file1)
        {
            string error = string.Empty;
            try
            {
                if (file1==null)
                    return JsonResponseModel.Error("Can't Find File",400);
                    var exts = _uploadFileService.GetListExtensionSupport();
                var ext = Path.GetFileName(file1.FileName);
                if (file1==null) return JsonResponseModel.Error("You have not selected a file!", 400);
                var file = Request.Form.Files[0];
                var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                using var fileStream = file.OpenReadStream();
                var responseFileModel = await _uploadFileService.UploadFile(User, filename, fileStream,out error);
                return JsonResponseModel.Success(responseFileModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("Upload Image: " + ex.Message);
                return JsonResponseModel.Error(ex.Message +  $"{error}", 204);
            }
        }

        /// <summary>
        /// John
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("update")]
        [Consumes("multipart/form-data")]
        public JsonResponseModel UploadImage([FromForm] FileUpdateRequest request)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Update image");
                GalleryService _service = new GalleryService(_config);
                string error = string.Empty;
                var file = request.FileToUpload;
                var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                using var fileStream = file.OpenReadStream();
                var data = _service.UploadFile(request.Id, filename, fileStream, User, out error);
                if (data)
                {
                    response.Status = 200;
                    response.Data = true;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Message = error;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Update Image: " + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_ERROR;
            }
            return response;
        }

        /// <summary>
        /// pttung1
        /// Get file by fileId
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpGet("GetFileById")]
        public JsonResponseModel GetFileById(long fileId)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Get file by fileId : " + fileId);
                GalleryService _service = new GalleryService(_config);
                var data = _service.Get(fileId);
                if (data != null)
                {
                    response.Status = 200;
                    response.Data = data;
                    response.Message = Constant.MSG_SUCCESS;
                }
                else
                {
                    response.Status = 100;
                    response.Message = Constant.MSG_DATA_NOT_FOUND;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Get file by fileId : " + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_DATA_NOT_FOUND;
            }
            return response;
        }

        /// <summary>
        /// Get Files by List ID
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        [HttpPost("GetListFiles")]
        public JsonResponseModel GetFileByIds(int[] _list)
        {
            GalleryService _service = new GalleryService(_config);
            return _service.GetFiles(_list);
        }
    }
}
