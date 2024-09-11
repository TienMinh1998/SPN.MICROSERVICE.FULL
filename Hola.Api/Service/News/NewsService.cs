using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.Repositories;
using Hola.Api.Service.BaseServices;


namespace Hola.Api.Service;

public class NewsService : BaseService<News>, INewsService
{
    private readonly INewsRepository _newsRepository;
    public NewsService(IRepository<News> baseReponsitory, INewsRepository newsRepository) : base(baseReponsitory)
    {
        _newsRepository = newsRepository;
    }
}
