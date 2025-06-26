using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Repositories;

public class SummaryQueryRepository(JobMagnetDbContext dbContext)
    : Repository<CareerHistory, long>(dbContext), ISummaryQueryRepository
{
    private IQueryable<CareerHistory> _query = dbContext.Set<CareerHistory>();

    public ISummaryQueryRepository IncludeWorkExperience()
    {
        _query = _query.Include(p => p.WorkExperiences);
        return this;
    }

    public ISummaryQueryRepository IncludeEducation()
    {
        _query = _query.Include(p => p.Qualifications);
        return this;
    }

    public async Task<IReadOnlyCollection<CareerHistory>> GetAllWithIncludesAsync() => await _query.ToListAsync().ConfigureAwait(false);

    public async Task<CareerHistory?> GetByIdWithIncludesAsync(long id)
    {
        return await _query.FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
    }
}