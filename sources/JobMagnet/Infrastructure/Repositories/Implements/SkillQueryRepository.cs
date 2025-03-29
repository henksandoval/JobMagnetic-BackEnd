using JobMagnet.Infrastructure.Context;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Repositories.Implements;

public class SkillQueryRepository(JobMagnetDbContext dbContext)
    : Repository<SkillEntity, long>(dbContext), ISkillQueryRepository
{
    private IQueryable<SkillEntity> _query = dbContext.Set<SkillEntity>();

    public SkillQueryRepository IncludeDetails()
    {
        _query = _query.Include(p => p.SkillDetails);
        return this;
    }

    public async Task<IReadOnlyCollection<SkillEntity>> GetAllWithIncludesAsync()
    {
        return await _query.ToListAsync().ConfigureAwait(false);
    }

    public async Task<SkillEntity?> GetByIdWithIncludesAsync(long id)
    {
        return await _query.FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
    }
}