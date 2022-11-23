using Hola.Core.Common;
using Hola.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UploadandDownloadFiles.Services;

namespace Hola.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1")]
    public class AssetsController : ControllerBase
    {
        private readonly ILogger<AssetsController> _logger;
        private readonly IOptions<SettingModel> _config;
        private readonly IFileService _fileService;
        private readonly string filepath;

        public AssetsController(ILogger<AssetsController> logger, IOptions<SettingModel> config, IFileService fileService)
        {
            _logger = logger;
            _config = config;
            _fileService = fileService;
        }

        [HttpGet("flags/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFlag(int id)
        {
            try
            {
                var image = System.IO.File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), $"assets/Flags/{id}.png"));
                return File(image, "image/png");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// nvmTien - Dowload document declaration 
        /// </summary>
        /// <param name="languageCode">Input code : en, ko ,es</param>
        /// <returns></returns>
        [HttpGet("docs/agent/declaration")]
        public IActionResult DownloadDeclaration(string languageCode)
        {
            try
            {
                _logger.LogInformation("Doc declaration Downloading");
                string filename = string.Empty;
                switch (languageCode.Trim().ToLower())
                {
                    case "en":
                        filename = "AgencyDeclarationForm_EN.docx"; break;
                    case "ko":
                        filename = "AgencyDeclarationForm_KR.docx"; break;
                    case "es":
                        filename = "AgencyDeclarationForm_ES.docx"; break;
                    default:
                        filename = "AgencyDeclarationForm_EN.docx"; break;
                }
                // Set URL : 
                string fileURL = Path.Combine(Directory.GetCurrentDirectory(), $"assets/Docs/{filename}");
                return File(System.IO.File.ReadAllBytes(fileURL), "application/octet-stream", filename);
            }
            catch (Exception ex)
            {
                _logger.LogError("Dowload document declaration: " + ex.ToString());
                return BadRequest();
            }

        }


        /// <summary>
        /// nvmTien -  Dowload document application
        /// </summary>
        /// <param name="languageCode">Input code : en, ko ,es</param>
        /// <returns></returns>
        [HttpGet("docs/agent/application")]
        public IActionResult DownloadApplication(string languageCode)
        {
            try
            {
                _logger.LogInformation("Doc Application Downloading");
                string filename = string.Empty;
                switch (languageCode.Trim().ToLower())
                {
                    case "en":
                        filename = "AgencyApplicationForm_EN.docx"; break;
                    case "ko":
                        filename = "AgencyApplicationFrom_KR.docx"; break;
                    case "es":
                        filename = "AgencyApplicationForm_ES.docx"; break;
                    default:
                        filename = "AgencyApplicationForm_EN.docx"; break;
                }
                string fileURL = Path.Combine(Directory.GetCurrentDirectory(), $"assets/Docs/{filename}");
                return File(System.IO.File.ReadAllBytes(fileURL), "application/octet-stream", filename);
            }
            catch (Exception ex)
            {
                _logger.LogError("Dowload document application: " + ex.ToString());
                return BadRequest();
            }

        }

        [HttpGet("docs/kyc/downloaddocument")]
        public IActionResult downloaddocument(string languageCode)
        {
            try
            {
                string filename = string.Empty;
                switch (languageCode.Trim().ToLower())
                {
                    case "en":
                        filename = "HolaPayOathProofread.docx"; break;
                    case "ko":
                        filename = "올라페이이용서약서.docx"; break;
                    default:
                        filename = "HolaPayOathProofread.docx"; break;
                }
                string fileURL = Path.Combine(Directory.GetCurrentDirectory(), $"assets/Docs/{filename}");
                return File(System.IO.File.ReadAllBytes(fileURL), "application/octet-stream", filename);
            }
            catch (Exception ex)
            {
                _logger.LogError("Download Kyc doc: " + ex.ToString());
                return BadRequest();
            }

        }

        /// <summary>
        /// Download Document Zip File
        /// </summary>
        /// <param name="formFiles"></param>
        /// <param name="subDirectory"></param>
        /// <returns></returns>
        [HttpPost(nameof(Upload))]
        public IActionResult Upload([Required] List<IFormFile> formFiles, [Required] string subDirectory)
        {
            try
            {
                _fileService.UploadFile(formFiles, subDirectory);
                return Ok(new { formFiles.Count, Size = _fileService.SizeConverter(formFiles.Sum(f => f.Length)) });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("dev/20091808/download/{id}")]
        [AllowAnonymous]
        public IActionResult download(string id)
        {
            try
            {
                string userid = User.Claims.FirstOrDefault(c => c.Type == ConstantUser.UserIDClaimKey).Value;
                string fileURL = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/weelpay/Document/{userid}/{id}");
                return File(System.IO.File.ReadAllBytes(fileURL), "application/octet-stream", id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Download Kyc doc: " + ex.ToString());
                return BadRequest();
            }

        }
    }
}