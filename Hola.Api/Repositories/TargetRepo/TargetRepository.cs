using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEFContext;
using Hola.Api.Repositories.UserRepository;

namespace Hola.Api.Repositories.TargetRepo
{
    public class TargetRepository : BaseRepository<Target>, ITargetRepository
    {
        public TargetRepository(EFContext DbContext) : base(DbContext)
        {
        }
    }
}
