using DatabaseCore.Infrastructure.ConfigurationEFContext;
using Hola.Core.Enums;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using DatabaseCore.Domain.Entities.Normals;

namespace Hola.Api.Repositories.UserRepository
{
    public class UserRepository : BaseRepository<Target>, IUserRepository
    {
        public UserRepository(EFContext dbContext) : base(dbContext)
        {

        }
   
    }
}
