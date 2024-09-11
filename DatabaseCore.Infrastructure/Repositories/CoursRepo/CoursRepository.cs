using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEFContext;
namespace DatabaseCore.Infrastructure.Repositories;

public class CoursRepository : BaseRepository<Cours>, ICoursRepository
{
    public CoursRepository(EFContext DbContext) : base(DbContext)
    {
    }
}

