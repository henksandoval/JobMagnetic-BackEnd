using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Domain.Ports.Repositories;

public interface ISummaryQueryRepository : IQueryRepository<CareerHistory, CareerHistoryId>
{
    Task<IReadOnlyCollection<CareerHistory>> GetAllWithIncludesAsync();
    Task<CareerHistory?> GetByIdWithIncludesAsync(CareerHistoryId id);
    ISummaryQueryRepository IncludeWorkExperience();
    ISummaryQueryRepository IncludeEducation();
}