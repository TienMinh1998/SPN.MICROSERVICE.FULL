namespace Hola.Api.Controllers;

using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Requests.Product;
using Hola.Api.Service;
using Hola.Core.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;



[Route("product")]
[ApiController]
public class ProductController
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("list")]
    public async Task<JsonResponseModel> GetList()
    {
        var products = await _productService.GetAllAsync();
        return JsonResponseModel.Success(products);
    }

    [HttpPost("add")]
    public async Task<JsonResponseModel> Add(ProductRequest model)
    {
        try
        {
            // Check product available 
            var old_product = _productService.GetFirstOrDefaultAsync(x => x.Name == model.Name && x.IsDelete == false);
            if (old_product != null) { return JsonResponseModel.Error("Sản phẩm đã tồn tại", 400); }
            Product product = new()
            {
                Name = model.Name,
                CreatedDate = DateTime.UtcNow,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Price = model.Price,
                Type = model.Type,
            };

            var response_adding = await _productService.AddAsync(product);
            return JsonResponseModel.Success(response_adding);
        }
        catch (Exception ex)
        {
            return JsonResponseModel.SERVER_ERROR(ex.Message);
        }
    }
}
