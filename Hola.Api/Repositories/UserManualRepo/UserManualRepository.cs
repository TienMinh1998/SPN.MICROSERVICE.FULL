using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEFContext;
using Hola.Api.Repositories.TargetRepo;

namespace Hola.Api.Repositories.UserManualRepo
{
    public class UserManualRepository : BaseRepository<UserManual>, IUserManualRepository
    {
        public UserManualRepository(EFContext DbContext) : base(DbContext)
        {
        }
    }
}
