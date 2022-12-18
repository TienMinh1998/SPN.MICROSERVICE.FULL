using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Repositories;
using Hola.Api.Service.BaseServices;
using Hola.Api.Service.TargetServices;

namespace Hola.Api.Service;

public class TopicService : BaseService<Topic>, ITopicService
{
    private readonly ITopicRepository _topicRepository;

    public TopicService(IRepository<Topic> baseReponsitory, 
        ITopicRepository topicRepository) : base(baseReponsitory)
    {
        _topicRepository = topicRepository;
    }
}
