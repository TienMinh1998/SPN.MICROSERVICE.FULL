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
        private readonly QuestionService _questionService;
        public CategoryController(IOptions<SettingModel> config, CategoryService categoryService, QuestionService questionService)
        {
            _config = config;
            this.categoryService = categoryService;
            _questionService = questionService;
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
        /// <summary>
        /// Lấy danh sách danh mục theo User
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet("GetCategories/{userid}")]
        public async Task<JsonResponseModel> GetAllCategory(int userid)
        {
            var result = await _questionService.GetAllCategory(userid);
            return JsonResponseModel.Success(result);
        }

    }
}
