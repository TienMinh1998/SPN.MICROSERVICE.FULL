using Hola.Api.Models.Questions;
using Hola.Api.Service;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Hola.Api.Models.Categories;

namespace Hola.Api.Controllers
{
    public class CategoryController
    {
        private readonly IOptions<SettingModel> _config;
        private readonly CategoryService categoryService;

        public CategoryController(IOptions<SettingModel> config, CategoryService categoryService)
        {
            _config = config;
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Add new Categories Service
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("AddCategory")]
        public async Task<JsonResponseModel> AddQuestion([FromBody] AddCategoryModel model)
        {
            var result = await categoryService.AddCategory(model);
            return JsonResponseModel.Success(result);
        }
        [HttpGet("Test")]
        public async Task<JsonResponseModel> AddQuestion()
        {
            return JsonResponseModel.Success("Service 2");
        }
    }
}
