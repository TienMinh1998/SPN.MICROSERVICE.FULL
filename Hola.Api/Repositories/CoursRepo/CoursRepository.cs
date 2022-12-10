using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEFContext;
using Hola.Api.Repositories.Grammar;

namespace Hola.Api.Repositories.CoursRepo
{
    public class CoursRepository : BaseRepository<Cours>, ICoursRepository
    {
        public CoursRepository(EFContext DbContext) : base(DbContext)
        {
        }
    }
}
