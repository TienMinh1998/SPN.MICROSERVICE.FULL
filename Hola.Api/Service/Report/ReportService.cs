using DatabaseCore.Domain.Entities.Normals;
using DatabaseCore.Infrastructure.Repositories;
using Hola.Api.Service.BaseServices;

namespace Hola.Api.Service;

public class ReportService : BaseService<Report>, IReportService
{
    private readonly IReportRepository _reportRepository;
    public ReportService(IRepository<Report> baseReponsitory, IReportRepository reportRepository) : base(baseReponsitory)
    {
        _reportRepository = reportRepository;
    }
}
