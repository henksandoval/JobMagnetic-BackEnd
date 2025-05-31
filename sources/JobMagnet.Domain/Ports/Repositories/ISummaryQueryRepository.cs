using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Domain.Ports.Repositories;

public interface ISummaryQueryRepository : IQueryRepository<SummaryEntity, long>
{
    Task<IReadOnlyCollection<SummaryEntity>> GetAllWithIncludesAsync();
    Task<SummaryEntity?> GetByIdWithIncludesAsync(long id);
    ISummaryQueryRepository IncludeWorkExperience();
    ISummaryQueryRepository IncludeEducation();
}