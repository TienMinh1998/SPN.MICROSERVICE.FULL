using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEFContext;
using Hola.Api.Repositories.TargetRepo;

namespace Hola.Api.Repositories;

public class TopicRepository : BaseRepository<Topic>, ITopicRepository
{
    public TopicRepository(EFContext DbContext) : base(DbContext)
    {
    }
}
