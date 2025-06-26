using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Domain.Ports.Repositories;

public interface ISummaryQueryRepository : IQueryRepository<CareerHistory, long>
{
    Task<IReadOnlyCollection<CareerHistory>> GetAllWithIncludesAsync();
    Task<CareerHistory?> GetByIdWithIncludesAsync(long id);
    ISummaryQueryRepository IncludeWorkExperience();
    ISummaryQueryRepository IncludeEducation();
}