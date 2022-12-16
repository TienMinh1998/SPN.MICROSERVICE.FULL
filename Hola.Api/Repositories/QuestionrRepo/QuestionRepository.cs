using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEFContext;
using Hola.Api.Repositories.TargetRepo;

namespace Hola.Api.Repositories.QuestionrRepo
{
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(EFContext DbContext) : base(DbContext)
        {
        }
    }
}
