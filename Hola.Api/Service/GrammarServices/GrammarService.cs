using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.Repositories;
using Hola.Api.Service.BaseServices;


namespace Hola.Api.Service.GrammarServices
{
    public class GrammarService : BaseService<Grammar>, IGrammarService
    {
        private readonly IGrammarRepository grammarRepository;
        public GrammarService(IRepository<Grammar> baseReponsitory, IGrammarRepository grammarRepository = null) : base(baseReponsitory)
        {
            this.grammarRepository = grammarRepository;
        }
    }
}
