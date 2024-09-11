using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.Repositories;
using Hola.Api.Service.BaseServices;


namespace Hola.Api.Service.CoursServices.V1;

public class CoursService : BaseService<Cours>, ICoursService
{
    private readonly ICoursRepository _coursRepository;
    public CoursService(IRepository<Cours> baseReponsitory, ICoursRepository coursRepository) : base(baseReponsitory)
    {
        _coursRepository = coursRepository;
    }
}
