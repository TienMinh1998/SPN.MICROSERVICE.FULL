using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEFContext;

namespace DatabaseCore.Infrastructure.Repositories;

public class QuestionStandardRepository : BaseRepository<QuestionStandard>, IQuestionStandardRepository
{

    public QuestionStandardRepository(EFContext DbContext) : base(DbContext)
    {
    }
}
