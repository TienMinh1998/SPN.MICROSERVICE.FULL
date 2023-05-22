using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEFContext;

namespace Hola.Api.Repositories;

public class PhraseRepository : BaseRepository<Phrase>, IPhraseRepository
{
    public PhraseRepository(EFContext DbContext) : base(DbContext)
    {
    }
}
