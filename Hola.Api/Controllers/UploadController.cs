using Hola.Api.Service.UploadFile;
using Hola.Api.Service.UploadFile.Model;
using Hola.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    public class UploadController : ControllerBase
    {
        private readonly IUploadFileService _uploadFileService;
        private readonly ILogger<UploadController> _logger;
        public UploadController(IUploadFileService uploadFileService, ILogger<UploadController> logger)
        {
            _uploadFileService = uploadFileService;
            _logger = logger;
        }
        /// <summary>
        /// Upload File to Image Folder, Author : Criss
        /// </summary>
        /// <param name="Files"></param>
        /// <returns></returns>
        [HttpPost("UploadFileToGoogleCloudStorage")]
        public async Task<JsonResponseModel> UploadFileToGoogleCloudStorage(IFormFile Files)
        {
            try
            {
                _uploadFileService.SettingEvironmentHttpContext(HttpContext);
                string url = await _uploadFileService.UploadImage(Files);
                _logger.LogError("TEST EVIRONMENT");
                return JsonResponseModel.Success(url);

            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR at UploadImage : " +  ex.Message);
                throw;
            }
            
        }

        [HttpGet("download")]
        public async Task<IActionResult> GetFlag()
        {
            try
            {
                var image = System.IO.File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/weelpay/0706092022233.docx"));
                return File(image, "image/png");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
