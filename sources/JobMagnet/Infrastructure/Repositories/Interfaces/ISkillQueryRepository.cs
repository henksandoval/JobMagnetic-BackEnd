using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Infrastructure.Repositories.Implements;

namespace JobMagnet.Infrastructure.Repositories.Interfaces;

public interface ISkillQueryRepository : IQueryRepository<SkillEntity, long>
{
    SkillQueryRepository IncludeDetails();
    Task<IReadOnlyCollection<SkillEntity>> GetAllWithIncludesAsync();
    Task<SkillEntity?> GetByIdWithIncludesAsync(long id);
}