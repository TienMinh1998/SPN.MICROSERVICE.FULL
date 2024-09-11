using Hola.Api.Service.BaseServices;
using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.Repositories;

namespace Hola.Api.Service.UserServices
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IRepository<User> baseReponsitory, IUserRepository userRepository) : base(baseReponsitory)
        {
            _userRepository = userRepository;
        }
    }
}
