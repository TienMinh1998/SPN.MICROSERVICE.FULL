using AutoMapper;
using Hola.Api.Service.CoursServices;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
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


        [HttpGet("getCours")]
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
    }
}
