//-----------------------------------
//  Author : Criss
//  Edit : Not yet
//  DoneDate : 29/06/2022
//  Content : KYC BO
//-----------------------------------

using Hola.Api.Attributes;
using Hola.Api.DTO.KYC;
using Hola.Api.DTO.KYC.KycItems;
using Hola.Api.DTO.Request;
using Hola.Api.Models;
using Hola.Api.Service;
using Hola.Core.Common;
using Hola.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    [ApiController]
    [Route("KYC/BO")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ApiExplorerSettings(GroupName = "BO")]
    public class KycWebController : ControllerBase
    {
        private readonly ILogger<KycWebController> _logger;
        private readonly IOptions<SettingModel> _config;
        private readonly KycWebService _kycWebService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IResponseCacheService _cacheservice;

        public KycWebController(ILogger<KycWebController> logger, IOptions<SettingModel> config = null, KycWebService kycWebService = null, IWebHostEnvironment webHostEnvironment = null, IResponseCacheService responseCacheService = null)
        {
            _logger = logger;
            _config = config;
            _kycWebService = kycWebService;
            _webHostEnvironment = webHostEnvironment;
            _cacheservice = responseCacheService;
        }

        /// <summary>
        /// Get List, Order by : UserId, Derection : ASC or DESC
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "searchText": "",
        ///        "PageNumber": 1,
        ///        "PageSize": 10,
        ///        "orderBy": "UserId",
        ///        "orderDirection": "DESC",
        ///        "startDate": "2022-06-22T06:47:34.249Z",
        ///        "endDate": "2022-06-22T06:47:34.249Z"
        ///     }
        ///
        /// </remarks>

        [HttpPost("GetListKyc")]
        public JsonResponseModel GetListKycs([FromBody] KycWebRequest model)
        {
            return _kycWebService.GetListKyc(model);
        }

        [HttpPost("GetKycByUserId/{UserId}")]
        public async Task<JsonResponseModel> GetListKycs(string UserId)
        {
            return await _kycWebService.GetKycByUserId(UserId);
        }
        /// <summary>
        /// Get KYC Detail By KYCId - Criss
        /// </summary>
        /// <param name="kycId"></param>
        /// <returns></returns>
        [HttpPost("GetKycDetail")]
        public async Task<JsonResponseModel> GetDetail(string kycId)
        {
            return await _kycWebService.GetKycDeatail(kycId);
        }

        /// <summary>
        /// Tải lên tài liệu của KYC
        /// </summary>
        /// <param name="inputFiles"></param>
        /// <returns></returns>
        [HttpPost("InsertDocument")]
        public JsonResponseModel UploadDocument(ICollection<IFormFile> inputFiles)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Upload doccument");
                var request = new AgentApplayRequest
                {
                    Documents = new List<AgentApplayDocumentRequest>()
                };
                foreach (var file1 in inputFiles)
                {
                    var reqItem = new AgentApplayDocumentRequest();
                    reqItem.Name = ContentDispositionHeaderValue.Parse(file1.ContentDisposition).FileName.Trim('"');
                    reqItem.Extension = Path.GetExtension(reqItem.Name);
                    string error = string.Empty;
                    GalleryService _service = new GalleryService(_config);
                    var fileResponse = _service.UploadFile(User, reqItem.Name, file1.OpenReadStream(), out error);

                    if (fileResponse == null) return JsonResponseModel.Response(100, string.Empty, error);

                    reqItem.Id = fileResponse.Id;
                    reqItem.Size = fileResponse.Size;
                    reqItem.FileUrl = fileResponse.FileUrl;
                    request.Documents.Add(reqItem);
                }
                _logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(response));
                return JsonResponseModel.Response(200, request, Constant.MSG_SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError("Upload Document: " + ex.Message);
                return JsonResponseModel.Response(100, null, ex.Message);
            }
        }

        /// <summary>
        /// Change Kyc Stage - Criss 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Type Of Document:
        ///
        ///     {
        ///        "KYCBankInfo": "0",
        ///        "KYCDeclarations": 1,
        ///        "KYCDocs": 2,
        ///        "KYCUserAddress": "3",
        ///        "KYCUserInfo": "4"
        ///     }
        ///
        /// </remarks>
        /// 
        [HttpPost("UpdateStatus")]
        public async Task<JsonResponseModel> UpdateStatus(UpdateStatusRequest request)
        {
            try
            {
                // Try Clear Cache
                await _cacheservice.RemoveRedisCacheAsync("/KYC/BO");
            }
            catch (Exception ex)
            {
                return JsonResponseModel.Error(ex.Message, SystemParam.ERRORCODE);
            }
            return await _kycWebService.UpdateStatus(request, User);
        }
        /// <summary>
        /// Change Stage of KYC
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Type Of Document:
        ///
        ///     {
        ///        "Applied": "0",
        ///        "Pending": 1,
        ///        "Approved": 2,
        ///        "Rejected": "3",
        ///        "Backlisted": "4"
        ///     }
        ///
        /// </remarks>
        /// 
        [HttpPost("ChangeKycStage")]
        public async Task<JsonResponseModel> ChangeKycStage(kycStageRequest request)
        {
            try
            {
                // Clear Cache
                await _cacheservice.RemoveRedisCacheAsync("/KYC/BO");
            }
            catch (Exception ex)
            {
                return JsonResponseModel.Error(ex.Message, SystemParam.ERRORCODE);
            }
            return await _kycWebService.ChangeKycStage(request,User);
        }
        /// <summary>
        /// On/Off KycStatus - Criss
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("OnOffStatusKyc")]
        public async Task<JsonResponseModel> OnOffKycStatus(OnOffRequest request)
        {
            try
            {
                // Clear Cache
                await _cacheservice.RemoveRedisCacheAsync("/KYC/BO");
            }
            catch (Exception ex)
            {
                return JsonResponseModel.Error(ex.Message, SystemParam.ERRORCODE);
            }
           
            return await _kycWebService.OnKycOfUser(request,User);
        }

        /// <summary>
        /// Get History 5 Step by KycId - Criss
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("History")]
        [Cache(100)]
        public JsonResponseModel GetHistory(KycHistoryRequest request)
        {
            return _kycWebService.GetHistory(request);
        }

        /// <summary>
        /// Update TrasactionTimeOfUser
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpPost("CoutTransactionTimeOfUser")]
        public JsonResponseModel CountTransactionOfUser(string UserId)
        {
            return _kycWebService.CountTransactionTimeOfUser(UserId);
        }

        /// <summary>
        /// Demo UPload File - CRISS
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("UploadFiles")]
        public async Task<JsonResponseModel> ComplainOrder([FromForm] StudentModels models)
        {
            try
            {
                var Images = await _kycWebService.UploadImages(SystemParam.FILE_NAME, HttpContext, _webHostEnvironment);
                return JsonResponseModel.Success(Images);
            }
            catch (Exception ex)
            {
                return JsonResponseModel.Error("Server Error! Can't not Upload, message :  " + ex.Message, SystemParam.ERRORCODE);
            }

        }

        /// <summary>
        /// Create new Note For KYC - Criss
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost("CreateNote")]
        public async Task<JsonResponseModel> CreateNote(KycNoteRequest requestModel)
        {
            try
            {
                await _cacheservice.RemoveRedisCacheAsync("/KYC/BO");
            }
            catch (Exception ex)
            {
                return JsonResponseModel.Error(ex.Message, SystemParam.ERRORCODE); throw;
            }
         
            return await _kycWebService.CreateNoteKyc(requestModel, User);

        }

        [HttpPost("GetNote")]
        public async Task<JsonResponseModel> GetNote(KycNoteDataRequest request)
        {
            return await _kycWebService.GetNote(request);
        }
    }
}
