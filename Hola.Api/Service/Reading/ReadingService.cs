using DatabaseCore.Domain.Entities.Normals;
using Hola.Api.Repositories;
using Hola.Api.Service.BaseServices;

namespace Hola.Api.Service;

public class ReadingService : BaseService<Reading>, IReadingService
{
    private readonly IReadingRepository _readingRepository;
    public ReadingService(IRepository<Reading> baseReponsitory, IReadingRepository readingRepository) : base(baseReponsitory)
    {
        _readingRepository = readingRepository;
    }
}
