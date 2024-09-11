using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.Repositories;
using Hola.Api.Service.BaseServices;


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
