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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ApplyAgentController : ControllerBase
    {
        private readonly ILogger<ApplyAgentController> _logger;
        private readonly IOptions<SettingModel> _config;
        private readonly IUploadFileService _uploadFileService;
        public ApplyAgentController(ILogger<ApplyAgentController> logger, IOptions<SettingModel> config, 
            IUploadFileService uploadFileService)
        {
            _logger = logger;
            _config = config;
            _uploadFileService = uploadFileService;
        }

        /// <summary>
        /// John
        /// </summary>
        /// <returns></returns>
        [HttpGet("history")]
        public JsonResponseModel ApplyAgentStatusHistoryByUser()
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Apply Agent Status History By User");
                ApplyAgentService _service = new ApplyAgentService(_config);

                var data = _service.ApplayAgentStatusHistory(User);
                if (data.Any())
                {
                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;
                    response.Data = data;
                }
                else if(data.Count() == 0)
                {
                    response.Status = 200;
                    response.Data = null;
                }
                else
                {
                    response.Status = 100;
                    response.Data = null;
                    response.Message = ErrorCodes.NullData;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Apply Agent Status History By User:" + ex.Message);
                response.Status = 100;
                response.Data = new ApplyStatusChangeResponse();
                response.Message = Constant.MSG_DATA_NOT_FOUND;
                return response;
            }
        }

        /// <summary>
        /// John
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("history/{id}")]
        public JsonResponseModel GetApplyStatusHistoryByApply(long id)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Apply Agent Status History By Apply");
                ApplyAgentService _service = new ApplyAgentService(_config);
                var data = _service.GetApplyStatusHistoryByApply(id);
                if (data.Any())
                {
                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;
                    response.Data = data;
                }
                else
                {
                    response.Status = 201;
                    response.Data = new ApplyStatusChangeResponse();
                    response.Message = ErrorCodes.NullData;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Apply Agent Status History By Apply:" + ex.Message);
                response.Status = 100;
                response.Data = new ApplyStatusChangeResponse();
                response.Message = Constant.MSG_DATA_NOT_FOUND;
                return response;
            }
        }

        [HttpPost("apply")]
        public JsonResponseModel Apply(AgentApplayRequest request)
        {
            try
            {
                ApplyAgentService _service = new ApplyAgentService(_config);
                var applyId = _service.ApplyForAgent(request, User);
                if (applyId != 0) return JsonResponseModel.Success(applyId, Constant.REQUEST_APPLY_SUCCESS);
                                  return JsonResponseModel.Success(applyId, Constant.REQUEST_ADDLY_SENDBEFORE);
            }
            catch (Exception ex)
            {
                _logger.LogError("Apply Agent: " + ex.Message);
                return JsonResponseModel.Error(Constant.MSG_SERVER_ERROR,Constant.SERVER_ERROR_CODE);
            }
        }

        [HttpPost("search")]
        public JsonResponseModel Search(AgentApplicationSearchRequest model)
        {
            JsonResponseModel response = new JsonResponseModel();
            try
            {
                _logger.LogInformation("Search apply");
                AgentApplicationService _service = new AgentApplicationService(_config);
                var data = _service.Search(model);
                if (data != null)
                {
                    response.Status = 200;
                    response.Message = Constant.MSG_SUCCESS;
                    response.Data = data;
                }
                else
                {
                    response.Status = 100;

                    response.Message = Constant.MSG_ERROR;
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError("Search apply: " + ex.Message);
                response.Status = 100;
                response.Message = Constant.MSG_DATA_NOT_FOUND;
            }
            return response;
        }

        [HttpPost("ApplyDocument")]
        [RequestSizeLimit(100_000_000)]
        public async  Task<JsonResponseModel> UploadDocument(ICollection<IFormFile> inputFiles)
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
                    var fileResponse = await _uploadFileService.UploadFile(User, reqItem.Name, file1.OpenReadStream(), out error);

                    if (fileResponse == null) return JsonResponseModel.Response(100, string.Empty, error);

                    reqItem.Id = fileResponse.Id;
                    reqItem.Size = fileResponse.Size;
                    reqItem.FileUrl = fileResponse.FileUrl;
                    request.Documents.Add(reqItem);
                }
                _logger.LogInformation(JsonSerializer.Serialize(response));
                return JsonResponseModel.Response(200, request, Constant.MSG_SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError("Upload Document: " + ex.Message);
                return JsonResponseModel.Response(100, null, ex.Message);
            }
        }
    }
}