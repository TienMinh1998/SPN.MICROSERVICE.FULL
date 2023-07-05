using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEFContext;

namespace Hola.Api.Repositories;

public class NewsRepository : BaseRepository<News>, INewsRepository
{
    public NewsRepository(EFContext DbContext) : base(DbContext)
    {
    }
}
