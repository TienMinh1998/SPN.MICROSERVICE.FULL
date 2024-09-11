using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEFContext;


namespace DatabaseCore.Infrastructure.Repositories;

public class TargetRepository : BaseRepository<Target>, ITargetRepository
{
    public TargetRepository(EFContext DbContext) : base(DbContext)
    {
    }
}
