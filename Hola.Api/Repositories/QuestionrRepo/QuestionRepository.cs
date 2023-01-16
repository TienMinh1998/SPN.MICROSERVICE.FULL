using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.ConfigurationEFContext;
using Hola.Api.Repositories.TargetRepo;
using System.Threading.Tasks;
using System.Linq;

namespace Hola.Api.Repositories.QuestionrRepo
{
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(EFContext DbContext) : base(DbContext)
        {
        }

        public async Task<int> CountQuestionLearnedToday(int userid)
        {
            return await Task.Run(() =>
             {
                 var model = (from qe in DbContext.Questions
                              where qe.fk_userid == userid && qe.created_on.AddDays(7).Day == System.DateTime.Now.Day
                              select qe.fk_userid).Count();
                 return model;
             });
        }
           
    }
}
