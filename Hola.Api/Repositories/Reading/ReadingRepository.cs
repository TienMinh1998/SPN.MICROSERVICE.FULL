using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEFContext;
using Hola.Api.Repositories.Grammar;

namespace Hola.Api.Repositories;
public class ReadingRepository : BaseRepository<Reading>, IReadingRepository
{
    public ReadingRepository(EFContext DbContext) : base(DbContext)
    {
    }
}
