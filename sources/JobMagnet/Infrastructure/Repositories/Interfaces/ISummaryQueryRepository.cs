using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Infrastructure.Repositories.Implements;

namespace JobMagnet.Infrastructure.Repositories.Interfaces;

public interface ISummaryQueryRepository : IQueryRepository<SummaryEntity, long>
{
    Task<IReadOnlyCollection<SummaryEntity>> GetAllWithIncludesAsync();
    Task<SummaryEntity?> GetByIdWithIncludesAsync(long id);
    SummaryQueryRepository IncludeWorkExperience();
    SummaryQueryRepository IncludeEducation();
}