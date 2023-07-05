using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Repositories;
using Hola.Api.Repositories.Grammar;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.GrammarServices;

namespace Hola.Api.Service;

public class NewsService : BaseService<News>, INewsService
{
    private readonly INewsRepository _newsRepository;
    public NewsService(IRepository<News> baseReponsitory, INewsRepository newsRepository) : base(baseReponsitory)
    {
        _newsRepository = newsRepository;
    }
}
