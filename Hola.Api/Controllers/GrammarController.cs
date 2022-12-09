using Hola.Api.Service.GrammarServices;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hola.Api.Controllers
{
    public class GrammarController : ControllerBase
    {

        private readonly IGrammarService _grammarService;

        public GrammarController(IGrammarService grammarService)
        {
            _grammarService = grammarService;
        }


        [HttpGet("GetAll")]
        public async Task<JsonResponseModel> GetAll()
        {
            var result = await _grammarService.GetAllAsync(x=>x.FK_UserId>0);
            return JsonResponseModel.Success(result);
        }
    }
}
