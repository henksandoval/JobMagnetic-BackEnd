using JobMagnet.Infrastructure.Context;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Repositories.Implements;

public class SummaryQueryRepository(JobMagnetDbContext dbContext)
    : Repository<SummaryEntity, long>(dbContext), ISummaryQueryRepository
{
    private IQueryable<SummaryEntity> _query = dbContext.Set<SummaryEntity>();

    public SummaryQueryRepository IncludeWorkExperience()
    {
        _query = _query.Include(p => p.WorkExperiences);
        return this;
    }

    public SummaryQueryRepository IncludeEducation()
    {
        _query = _query.Include(p => p.Education);
        return this;
    }

    public async Task<IReadOnlyCollection<SummaryEntity>> GetAllWithIncludesAsync()
    {
        return await _query.ToListAsync().ConfigureAwait(false);
    }

    public async Task<SummaryEntity?> GetByIdWithIncludesAsync(long id)
    {
        return await _query.FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
    }
}