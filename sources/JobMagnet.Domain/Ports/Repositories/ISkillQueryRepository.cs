using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Domain.Ports.Repositories;

public interface ISkillQueryRepository : IQueryRepository<SkillEntity, long>
{
    ISkillQueryRepository IncludeDetails();
    Task<IReadOnlyCollection<SkillEntity>> GetAllWithIncludesAsync();
    Task<SkillEntity?> GetByIdWithIncludesAsync(long id);
}