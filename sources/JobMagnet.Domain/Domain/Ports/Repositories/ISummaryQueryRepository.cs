using JobMagnet.Domain.Domain.Ports.Repositories.Base;
using JobMagnet.Domain.Entities;

namespace JobMagnet.Domain.Domain.Ports.Repositories;

public interface ISummaryQueryRepository : IQueryRepository<SummaryEntity, long>
{
    Task<IReadOnlyCollection<SummaryEntity>> GetAllWithIncludesAsync();
    Task<SummaryEntity?> GetByIdWithIncludesAsync(long id);
    ISummaryQueryRepository IncludeWorkExperience();
    ISummaryQueryRepository IncludeEducation();
}