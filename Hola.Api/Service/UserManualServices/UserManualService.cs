using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.Repositories;

using Hola.Api.Service.BaseServices;


namespace Hola.Api.Service.UserManualServices
{
    public class UserManualService : BaseService<UserManual>, IUserManualService
    {
        private IUserManualRepository _repository;
        public UserManualService(IRepository<UserManual> baseReponsitory, IUserManualRepository repository) : base(baseReponsitory)
        {
            _repository = repository;
        }
    }
}
