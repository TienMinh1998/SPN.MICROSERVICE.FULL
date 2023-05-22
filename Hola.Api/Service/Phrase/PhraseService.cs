using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Repositories;
using Hola.Api.Service.BaseServices;

namespace Hola.Api.Service;

public class PhraseService : BaseService<Phrase>, IPhraseService
{
    private readonly IPhraseRepository _phraseRepository;
    public PhraseService(IRepository<Phrase> baseReponsitory, IPhraseRepository phraseRepository) : base(baseReponsitory)
    {
        _phraseRepository = phraseRepository;
    }
}
