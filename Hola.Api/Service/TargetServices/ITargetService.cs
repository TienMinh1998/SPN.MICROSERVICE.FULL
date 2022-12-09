using EntitiesCommon.EntitiesModel;
using EntitiesCommon.Requests.TargetRequests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hola.Api.Service.TargetServices
{
    public interface ITargetService
    {
        Task<List<TargetModel>> GetList(int userid);
        Task<TargetModel> AddAsync(TargetModel target);

        Task<TargetModel> GetById(int Id, int userid);
    }
}