using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Repositories;

public class SkillQueryRepository(JobMagnetDbContext dbContext)
    : Repository<SkillEntity, long>(dbContext), ISkillQueryRepository
{
    private IQueryable<SkillEntity> _query = dbContext.Set<SkillEntity>();

    public ISkillQueryRepository IncludeDetails()
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