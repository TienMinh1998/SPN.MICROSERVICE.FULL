using AutoMapper;
using Azure;
using DatabaseCore.Domain.Entities.Normals;
using EntitiesCommon.EntitiesModel;
using EntitiesCommon.Requests.GrammarRequests;
using Hola.Api.Service.GrammarServices;
using Hola.Api.Service.UserManualServices;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    [Route("v1")]
    public class GrammarController : ControllerBase
    {

        private readonly IGrammarService _grammarService;
        private readonly IUserManualService userManualService;

        private readonly IMapper _mapperService;

        public GrammarController(IGrammarService grammarService, IMapper mapperService,
            IUserManualService userManualService)
        {
            _grammarService = grammarService;
            _mapperService = mapperService;
            this.userManualService = userManualService;
        }

        [HttpGet("listgrammar")]
        public async Task<JsonResponseModel> GetAll()
        {
            var result = await _grammarService.GetAllAsync();
            return JsonResponseModel.Success(result);
        }

        [HttpGet("Get_Grammar_By_Id/{code}")]
        public async Task<JsonResponseModel> Get_Grammar_By_Id(string code)
        {
            var result = await _grammarService.GetFirstOrDefaultAsync(x => x.Code == code);
            var detail = await userManualService.GetAllAsync(x => x.Fk_Grannar_Id == result.PK_grammarId);

            Dictionary<string, object> dic_response = new Dictionary<string, object>();
            dic_response.Add("overview", result);
            dic_response.Add("usermanual", detail);
            return JsonResponseModel.Success(dic_response);
        }
        [HttpPost("AddGrammarUserManual")]
        public async Task<JsonResponseModel> AddGrammarUserManual([FromBody] UserManualModel model)
        {
            try
            {
                var entityUserManual = _mapperService.Map<UserManual>(model);
                var response = await userManualService.AddAsync(entityUserManual);
                return JsonResponseModel.Success(response);
            }
            catch (Exception ex)
            {

                return JsonResponseModel.Error(ex.Message, 500);
            }

        }

        [HttpPut("grammar")]
        public async Task<JsonResponseModel> AddGrammar([FromBody] AddGrammarRequest model)
        {
            try
            {
                var _addValue = await _grammarService.GetFirstOrDefaultAsync(x => x.Code == model.Code);
                if (_addValue == null)
                {
                    var request = _mapperService.Map<Grammar>(model);
                    request.created_on = DateTime.Now;
                    var response = await _grammarService.AddAsync(request);
                    return JsonResponseModel.Success(response);
                }
                else
                {
                    return JsonResponseModel.Error("Ngữ Pháp đã tồn tại", 400);
                }

            }
            catch (Exception ex)
            {

                return JsonResponseModel.Error(ex.Message, 500);
            }

        }

        [HttpDelete("grammar")]
        public async Task<JsonResponseModel> Delete(int id)
        {
            try
            {

                var grammar = await _grammarService.GetFirstOrDefaultAsync(x => x.PK_grammarId == id);
                await _grammarService.DeleteAsync(grammar);
                return JsonResponseModel.Success("Xóa thành công " + grammar.grammar_name);
            }
            catch (Exception ex)
            {

                return JsonResponseModel.Error(ex.Message, 500);
            }

        }


    }
}
