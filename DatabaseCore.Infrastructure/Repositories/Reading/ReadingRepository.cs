using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEFContext;

namespace DatabaseCore.Infrastructure.Repositories;
public class ReadingRepository : BaseRepository<Reading>, IReadingRepository
{
    public ReadingRepository(EFContext DbContext) : base(DbContext)
    {
    }
}
