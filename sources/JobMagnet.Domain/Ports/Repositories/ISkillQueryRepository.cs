using JobMagnet.Domain.Domain.Ports.Repositories.Base;
using JobMagnet.Domain.Entities;

namespace JobMagnet.Domain.Domain.Ports.Repositories;

public interface ISkillQueryRepository : IQueryRepository<SkillEntity, long>
{
    ISkillQueryRepository IncludeDetails();
    Task<IReadOnlyCollection<SkillEntity>> GetAllWithIncludesAsync();
    Task<SkillEntity?> GetByIdWithIncludesAsync(long id);
}