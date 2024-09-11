using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.Repositories;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.TargetServices;
using System.Threading.Tasks;

namespace Hola.Api.Service.V1;

public class QuestionService : BaseService<Question>, IQuestionService
{
    private readonly IQuestionRepository _repository;
    private readonly DapperBaseService _dapper;

    public QuestionService(IRepository<Question> baseReponsitory,
        IQuestionRepository repository,
        DapperBaseService dapper) : base(baseReponsitory)
    {
        _repository = repository;
        _dapper = dapper;
    }
    public async Task<int> CountQuestionToday(int UserID)
    {
        try
        {
            string query = $"SELECT count(1) FROM usr.question where fk_userid = {UserID} and created_on::TIMESTAMP::DATE = CURRENT_DATE::TIMESTAMP::DATE;";
            var response = _dapper.QueryFirstOrDefault<int>(query);
            return response;
        }
        catch (System.Exception)
        {
            return await Task.FromResult(0);
        }

    }
}
