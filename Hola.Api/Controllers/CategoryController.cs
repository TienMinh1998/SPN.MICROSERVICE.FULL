using Hola.Api.Models.Questions;
using Hola.Api.Service;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Hola.Api.Models.Categories;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Hola.Api.Controllers;

public class CategoryController : ControllerBase
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
    [Authorize]
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
    [HttpGet("GetCategories")]
    [Authorize]
    public async Task<JsonResponseModel> GetAllCategory()
    {
        string userid = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
        var result = await _questionService.GetAllCategory(int.Parse(userid));
        if (result==null || result.Count<=0 )
        {
            return JsonResponseModel.Error("No Data", 199);
        }
        else
        {
            return JsonResponseModel.Success(result);
         
        }
    }
       

}
