using DatabaseCore.Infrastructure.ConfigurationEFContext;
using Hola.Api.Repositories.Grammar;
using Hola.Api.Repositories.UserRepository;
namespace Hola.Api.Repositories;

public class GrammarRepository : BaseRepository<DatabaseCore.Domain.Entities.Normals.Grammar>, IGrammarRepository
{
    public GrammarRepository(EFContext DbContext) : base(DbContext)
    {
    }
}
