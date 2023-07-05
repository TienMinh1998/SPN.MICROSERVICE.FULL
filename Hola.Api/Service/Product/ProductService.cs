using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Repositories;
using Hola.Api.Service.BaseServices;

namespace Hola.Api.Service;

public class ProductService : BaseService<Product>, IProductService
{
    private readonly IProductRepository _repository;
    public ProductService(IRepository<Product> baseReponsitory, IProductRepository repository) : base(baseReponsitory)
    {
        _repository = repository;
    }
}
