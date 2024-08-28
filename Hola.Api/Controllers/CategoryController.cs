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
using MediatR;
using SPNApplication.Commnands;
using Hola.Api.Common;
using Hola.Api.Attributes;

namespace Hola.Api.Controllers;

[Route("categories")]
public class CategoryController : ControllerBase
{
    private readonly IOptions<SettingModel> _config;
    private readonly CategoryService categoryService;
    private readonly QuestionService _questionService;
    private readonly ICategoryService _service;
    private readonly IMediator _mediator;
    public CategoryController(IOptions<SettingModel> config, CategoryService categoryService, QuestionService questionService, ICategoryService service = null, IMediator mediator = null)
    {
        _config = config;
        this.categoryService = categoryService;
        _questionService = questionService;
        _service = service;
        _mediator = mediator;
    }

    [HttpPost("test_get_way")]
    [Permission(Permissions = new PermissionKeyNames[] { PermissionKeyNames.READ })]
    public async Task<JsonResponseModel> AddQuestion()
    {
        int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        return JsonResponseModel.Success("Test OK");
    }


    /// <summary>
    /// Add new Categories Service
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("AddCategory")]
    public async Task<JsonResponseModel> AddQuestion([FromBody] AddCategoryModel model)
    {
        // int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        int userid = 123;
        //var result = await categoryService.AddCategory(model, userid);
        AddCatagoriesCommand command = new()
        {
            userid = userid,
            Define = model.Define,
            Image = model.Image,
            Name = model.Name,
        };
        var result = await _mediator.Send(command);
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
        int userid = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        var result = await _service.GetAllAsync(x => x.fk_userid == userid);
        result = result.OrderByDescending(x => x.totalquestion).ThenByDescending(x => x.created_on).ToList();
        return JsonResponseModel.Success(result);
    }

}
