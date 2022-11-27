using Hola.Api.Repositories.UserRepository;
using Hola.Api.Repositories;
using Hola.Api.Service.BaseServices;
using Hola.Core.Enums;
using Hola.Core.Model.CommonModel;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using DatabaseCore.Domain.Entities.Normals;

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
