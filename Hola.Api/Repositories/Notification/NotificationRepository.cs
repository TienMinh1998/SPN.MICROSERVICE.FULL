using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEFContext;
using Hola.Api.Repositories.Grammar;
using System.Threading.Tasks;

namespace Hola.Api.Repositories;

public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
{
    public NotificationRepository(EFContext DbContext) : base(DbContext)
    {

    }
}
