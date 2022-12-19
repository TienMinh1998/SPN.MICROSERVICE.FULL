using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Service.BaseServices;
using System.Threading.Tasks;

namespace Hola.Api.Service.V1;


public interface IQuestionService : IBaseService<Question>
{
    Task<int> CountQuestionToday(int UserID);
}
