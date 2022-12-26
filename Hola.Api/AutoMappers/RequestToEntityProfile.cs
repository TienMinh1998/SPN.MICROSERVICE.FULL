using AutoMapper;
using DatabaseCore.Domain.Entities.Normals;
using EntitiesCommon.EntitiesModel;
using EntitiesCommon.Requests.GrammarRequests;
using EntitiesCommon.Requests.TargetRequests;
using Hola.Api.Models;
using Hola.Api.Requests;

namespace Hola.Api.AutoMappers
{
    public class RequestToEntityProfile : Profile
    {
        public RequestToEntityProfile()
        {
            // Model -> Entity
            CreateMap<AddTargetRequest, Target>();
            CreateMap<AddGrammarRequest, Grammar>();
            CreateMap<UserManualModel, UserManual>();
            CreateMap<CourseModel, Cours>();
            CreateMap<UpdateCourseModel, Cours>();
            CreateMap<AddTopicModel, Topic>();
            CreateMap<AddQuestionStandardModel, QuestionStandard>();

            // Entity -> Model
        }
    }
}
