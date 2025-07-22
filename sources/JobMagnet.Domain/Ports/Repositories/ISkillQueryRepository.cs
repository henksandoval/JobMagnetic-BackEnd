using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Domain.Ports.Repositories;

public interface ISkillQueryRepository : IQueryRepository<SkillSet, long>
{
    ISkillQueryRepository IncludeDetails();
    Task<IReadOnlyCollection<SkillSet>> GetAllWithIncludesAsync();
    Task<SkillSet?> GetByIdWithIncludesAsync(long id);
}