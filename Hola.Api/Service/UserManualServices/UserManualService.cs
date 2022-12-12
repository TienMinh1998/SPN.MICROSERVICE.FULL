using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Repositories;
using Hola.Api.Repositories.UserManualRepo;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.UserServices;

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
