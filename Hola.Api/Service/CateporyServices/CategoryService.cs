using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.Repositories;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.CoursServices;

namespace Hola.Api.Service.CateporyServices.v1;

public class CategoryService : BaseService<Category>, ICategoryService
{
    private readonly IRepository<Category> _repository;
    public CategoryService(IRepository<Category> baseReponsitory,
        IRepository<Category> repository) : base(baseReponsitory)
    {
        _repository = repository;
    }
}
