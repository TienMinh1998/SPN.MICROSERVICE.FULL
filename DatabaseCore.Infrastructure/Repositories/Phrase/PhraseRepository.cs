using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEFContext;
namespace DatabaseCore.Infrastructure.Repositories;

public class PhraseRepository : BaseRepository<Phrase>, IPhraseRepository
{
    public PhraseRepository(EFContext DbContext) : base(DbContext)
    {
    }
}
