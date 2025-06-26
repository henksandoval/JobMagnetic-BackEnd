using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Repositories;

public class SummaryQueryRepository(JobMagnetDbContext dbContext)
    : Repository<ProfessionalSummary, long>(dbContext), ISummaryQueryRepository
{
    private IQueryable<ProfessionalSummary> _query = dbContext.Set<ProfessionalSummary>();

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

    public async Task<IReadOnlyCollection<ProfessionalSummary>> GetAllWithIncludesAsync() => await _query.ToListAsync().ConfigureAwait(false);

    public async Task<ProfessionalSummary?> GetByIdWithIncludesAsync(long id)
    {
        return await _query.FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
    }
}