using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Repositories;
using Hola.Api.Repositories.Grammar;
using Hola.Api.Repositories.UserRepository;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.UserServices;

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
