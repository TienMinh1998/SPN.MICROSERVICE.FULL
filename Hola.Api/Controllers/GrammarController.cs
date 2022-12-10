using AutoMapper;
using DatabaseCore.Domain.Entities.Normals;
using EntitiesCommon.Requests.GrammarRequests;
using Hola.Api.Service.GrammarServices;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    public class GrammarController : ControllerBase
    {

        private readonly IGrammarService _grammarService;
        private readonly IMapper _mapperService;

        public GrammarController(IGrammarService grammarService, IMapper mapperService)
        {
            _grammarService = grammarService;
            _mapperService = mapperService;
        }
        [HttpGet("AddGrammar")]
        public async Task<JsonResponseModel> AddGrammar([FromBody] AddGrammarRequest model)
        {
            try
            {
                var request = _mapperService.Map<Grammar>(model);
                request.created_on = DateTime.Now;
                var response = await _grammarService.AddAsync(request);
                return JsonResponseModel.Success(response);
            }
            catch (Exception ex)
            {

                return JsonResponseModel.Error(ex.Message, 500);
            }
           
        }

        [HttpGet("GetAll")]
        public async Task<JsonResponseModel> GetAll()
        {
            var result = await _grammarService.GetAllAsync(x=>x.FK_UserId>0);
            return JsonResponseModel.Success(result);
        }
    }
}
