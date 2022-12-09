using AutoMapper;
using EntitiesCommon.EntitiesModel;
using EntitiesCommon.Requests.TargetRequests;

namespace Hola.Api.AutoMappers
{
    public class RequestToEntityProfile : Profile
    {
        public RequestToEntityProfile()
        {
            CreateMap<AddTargetRequest, TargetModel>();
        }
    }
}
