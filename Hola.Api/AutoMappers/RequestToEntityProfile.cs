using AutoMapper;
using DatabaseCore.Domain.Entities.Normals;
using EntitiesCommon.EntitiesModel;
using EntitiesCommon.Requests.GrammarRequests;
using EntitiesCommon.Requests.TargetRequests;

namespace Hola.Api.AutoMappers
{
    public class RequestToEntityProfile : Profile
    {
        public RequestToEntityProfile()
        {
            CreateMap<AddTargetRequest, Target>();
            CreateMap<AddGrammarRequest, Grammar>();
            CreateMap<UserManualModel, UserManual>();
        }
    }
}
