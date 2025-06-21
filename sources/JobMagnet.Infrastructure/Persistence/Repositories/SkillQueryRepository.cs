using JobMagnet.Domain.Core.Entities.Skills;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Repositories;

public class SkillQueryRepository(JobMagnetDbContext dbContext)
    : Repository<SkillSet, long>(dbContext), ISkillQueryRepository
{
    private IQueryable<SkillSet> _query = dbContext.Set<SkillSet>();

    public ISkillQueryRepository IncludeDetails()
    {
        _query = _query.Include(p => p.Skills);
        return this;
    }

    public async Task<IReadOnlyCollection<SkillSet>> GetAllWithIncludesAsync()
    {
        return await _query.ToListAsync().ConfigureAwait(false);
    }

    public async Task<SkillSet?> GetByIdWithIncludesAsync(long id)
    {
        return await _query.FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
    }
}