using Hola.Api.Models.Questions;
using Hola.Api.Service;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Hola.Api.Models.Categories;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Hola.Api.Service.CateporyServices;
using Hola.Api.Attributes;
using Hola.Api.Models.News;
using DatabaseCore.Domain.Entities.Normals;
using System;
using Hola.Api.Requests.News;
using Hola.Api.Common;

namespace Hola.Api.Controllers
{
    [Route("news")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet("list")]
        public async Task<JsonResponseModel> GetList()
        {
            var news = await _newsService.GetAllAsync();
            return JsonResponseModel.Success(news);
        }


        [HttpPost("addnews")]
        [Auth]
        public async Task<JsonResponseModel> Add(NewsAddingRequest model)
        {
            try
            {
                // KIểm tra xem tin đã tồn tại hay chưa
                var old_news = await _newsService.GetFirstOrDefaultAsync(x => x.Title == model.Title);
                if (old_news != null)
                {
                    return JsonResponseModel.Error("Tin đã tồn tại", 400);
                }

                // Thêm vào cơ sở dữ liệu 
                News entity = new()
                {
                    CategoryId = model.CategoryId,
                    Content = model.Content,
                    CreatedDate = DateTime.UtcNow,
                    ImageUrl = model.ImageUrl,
                    Title = model.Title
                };
                var res = await _newsService.AddAsync(entity);
                return JsonResponseModel.Success(res);
            }
            catch (Exception ex)
            {
                return JsonResponseModel.SERVER_ERROR(ex.Message);
            }

        }
    }
}
