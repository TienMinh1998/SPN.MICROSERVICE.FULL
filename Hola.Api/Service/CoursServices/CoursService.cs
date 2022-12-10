using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Repositories;
using Hola.Api.Repositories.CoursRepo;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.GrammarServices;

namespace Hola.Api.Service.CoursServices
{
    public class CoursService : BaseService<Cours>, ICoursService
    {
        private readonly ICoursRepository _coursRepository;
        public CoursService(IRepository<Cours> baseReponsitory, ICoursRepository coursRepository) : base(baseReponsitory)
        {
            _coursRepository = coursRepository;
        }
    }
}
