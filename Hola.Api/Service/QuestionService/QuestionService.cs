using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Repositories;
using Hola.Api.Repositories.QuestionrRepo;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.TargetServices;

namespace Hola.Api.Service.V1;

public class QuestionService : BaseService<Question>, IQuestionService
{
    private readonly IQuestionRepository _repository;
    public QuestionService(IRepository<Question> baseReponsitory,
        IQuestionRepository repository) : base(baseReponsitory)
    {
        _repository = repository;
    }
}
