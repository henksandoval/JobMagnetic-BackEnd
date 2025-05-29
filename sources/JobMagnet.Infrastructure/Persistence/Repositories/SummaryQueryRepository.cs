using JobMagnet.Domain.Domain.Ports.Repositories;
using JobMagnet.Domain.Entities;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Repositories;

public class SummaryQueryRepository(JobMagnetDbContext dbContext)
    : Repository<SummaryEntity, long>(dbContext), ISummaryQueryRepository
{
    private IQueryable<SummaryEntity> _query = dbContext.Set<SummaryEntity>();

    public ISummaryQueryRepository IncludeWorkExperience()
    {
        _query = _query.Include(p => p.WorkExperiences);
        return this;
    }

    public ISummaryQueryRepository IncludeEducation()
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