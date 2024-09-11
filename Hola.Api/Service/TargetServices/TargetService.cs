using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.Repositories;
using Hola.Api.Service.BaseServices;


namespace Hola.Api.Service.TargetServices;


public class TargetService : BaseService<Target>, ITargetService
{
    private ITargetRepository _targetRepository;

    public TargetService(IRepository<Target> baseReponsitory,
        ITargetRepository targetRepository) : base(baseReponsitory)
    {
        _targetRepository = targetRepository;
    }
}
