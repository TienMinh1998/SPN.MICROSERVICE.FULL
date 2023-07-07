namespace Hola.Api.Controllers;

using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Attributes;
using Hola.Api.Common;
using Hola.Api.Requests.Product;
using Hola.Api.Requests.Reading;
using Hola.Api.Service;
using Hola.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API.Models;
using System;
using System.Threading.Tasks;



[Route("product")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IUploadFileService _uploadService;
    public ProductController(IProductService productService, IUploadFileService uploadService)
    {
        _productService = productService;
        _uploadService = uploadService;
    }

    /// <summary>
    /// Lấy ra danh sách sản phẩm
    /// </summary>
    /// <returns></returns>
    [HttpGet("list")]
    public async Task<JsonResponseModel> GetList()
    {
        var products = await _productService.GetAllAsync();
        return JsonResponseModel.Success(products);
    }

    [HttpPost("add")]
    [Auth]
    public async Task<JsonResponseModel> Add([FromForm] ProductRequest model)
    {
        try
        {
            // Add Image
            string url = await _uploadService.UploadImage(model.file, HttpContext);

            // Check product available 
            var old_product = await _productService.GetFirstOrDefaultAsync(x => x.Name == model.Name && x.IsDelete == false);
            if (old_product != null) { return JsonResponseModel.Error("Sản phẩm đã tồn tại", 400); }
            Product product = new()
            {
                Name = model.Name,
                CreatedDate = DateTime.UtcNow,
                Description = model.Description,
                ImageUrl = url,
                Price = model.Price
            };

            var response_adding = await _productService.AddAsync(product);
            return JsonResponseModel.Success(response_adding);
        }
        catch (Exception ex)
        {
            return JsonResponseModel.SERVER_ERROR(ex.Message);
        }
    }


    /// <summary>
    /// Lấy chi tiết của sản phẩm
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<JsonResponseModel> getDetail(int id)
    {
        var products = await _productService.GetFirstOrDefaultAsync(x => x.Id == id);
        return JsonResponseModel.Success(products);
    }

    /// <summary>
    /// Xóa sản phẩm, xóa thẳng, không lưu lại gì cả
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Auth]
    public async Task<JsonResponseModel> Delete(int id)
    {
        try
        {
            var entity = await _productService.GetFirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                var response = _productService.DeleteAsync(entity);
                return JsonResponseModel.Success(response);
            }
            else
            {
                return JsonResponseModel.Error($"Không tìm thấy bản ghi có 'id ={id}'", 400);
            }
        }
        catch (Exception ex)
        {
            return JsonResponseModel.SERVER_ERROR(ex.Message);
        }
    }

}
