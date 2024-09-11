using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.Repositories;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.V1;

namespace Hola.Api.Service;

public class NotificationService : BaseService<Notification>, INotificationService
{
    private readonly INotificationRepository _repository;
    public NotificationService(IRepository<Notification> baseReponsitory,
        INotificationRepository repository) : base(baseReponsitory)
    {
        _repository = repository;
    }
}
