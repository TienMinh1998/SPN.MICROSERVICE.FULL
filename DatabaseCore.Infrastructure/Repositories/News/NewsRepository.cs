using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEFContext;

namespace DatabaseCore.Infrastructure.Repositories;

public class NewsRepository : BaseRepository<News>, INewsRepository
{
    public NewsRepository(EFContext DbContext) : base(DbContext)
    {
    }
}
