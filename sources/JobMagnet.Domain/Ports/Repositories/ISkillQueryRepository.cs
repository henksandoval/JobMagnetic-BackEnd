using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Domain.Ports.Repositories;

public interface ISkillQueryRepository : IQueryRepository<SkillSetEntity, long>
{
    ISkillQueryRepository IncludeDetails();
    Task<IReadOnlyCollection<SkillSetEntity>> GetAllWithIncludesAsync();
    Task<SkillSetEntity?> GetByIdWithIncludesAsync(long id);
}