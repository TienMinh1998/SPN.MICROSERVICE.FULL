using DatabaseCore.Domain.Entities.Normals;
using System.Threading.Tasks;

namespace DatabaseCore.Infrastructure.Repositories;

public interface IQuestionRepository : IRepository<Question>
{
    Task<int> CountQuestionLearnedToday(int userid);

}
