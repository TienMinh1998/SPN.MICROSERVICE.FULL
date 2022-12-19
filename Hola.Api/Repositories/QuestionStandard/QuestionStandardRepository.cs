using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEFContext;
using Hola.Api.Repositories.TargetRepo;
using Hola.Api.Service;

namespace Hola.Api.Repositories;

public class QuestionStandardRepository : BaseRepository<QuestionStandard>, IQuestionStandardRepository
{
   
    public QuestionStandardRepository(EFContext DbContext) : base(DbContext)
    {
    }
}
