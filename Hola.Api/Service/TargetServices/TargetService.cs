using DatabaseCore.Domain.Entities.Normals;
using EntitiesCommon.EntitiesModel;
using EntitiesCommon.Requests.TargetRequests;
using Hola.Api.Models;
using Hola.Api.Repositories;
using Hola.Api.Repositories.TargetRepo;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.UserServices;
using Hola.Core.Common;
using Hola.Core.Model;
using Hola.Core.Service;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hola.Api.Service.TargetServices;


public class TargetService : BaseService<Target>, ITargetService
{
    private ITargetRepository _targetRepository;

    public TargetService(IRepository<Target> baseReponsitory, 
        ITargetRepository targetRepository) : base(baseReponsitory)
    {
        _targetRepository = targetRepository;
    }
}
