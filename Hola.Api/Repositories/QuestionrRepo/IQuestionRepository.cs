using DatabaseCore.Domain.Entities.Normals;
using System.Threading.Tasks;

namespace Hola.Api.Repositories.QuestionrRepo
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<int> CountQuestionLearnedToday(int userid);  

    }
}
