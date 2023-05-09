using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Repositories;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.V1;

namespace Hola.Api.Service;
public class QuestionStandardService : BaseService<QuestionStandard>, IQuestionStandardService
{
    private readonly IQuestionStandardRepository _questionStandardRepository;
    public QuestionStandardService(IRepository<QuestionStandard> baseReponsitory,
        IQuestionStandardRepository questionStandardRepository) : base(baseReponsitory)
    {
        _questionStandardRepository = questionStandardRepository;
    }
}

