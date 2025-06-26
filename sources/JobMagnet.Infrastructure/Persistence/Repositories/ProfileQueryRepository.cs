using System.Linq.Expressions;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Repositories;

public class ProfileQueryRepository(JobMagnetDbContext dbContext)
    : Repository<Profile, long>(dbContext), IProfileQueryRepository
{
    private IQueryable<Profile> _query = dbContext.Profiles;
    private Expression<Func<Profile, bool>> _whereCondition = x => true;

    public IProfileQueryRepository WithResume()
    {
        _query = _query
            .Include(p => p.Resume)
            .ThenInclude(r => r.ContactInfo)
            .ThenInclude(c => c.ContactType);

        return this;
    }

    public IProfileQueryRepository WithSkills()
    {
        _query = _query
            .Include(p => p.SkillSet)
            .ThenInclude(s => s.Skills);

        return this;
    }

    public IProfileQueryRepository WithSummary()
    {
        _query = _query
            .Include(p => p.ProfessionalSummary)
            .ThenInclude(p => p.Qualifications);
        _query = _query
            .Include(p => p.ProfessionalSummary)
            .ThenInclude(p => p.WorkExperiences)
            .ThenInclude(p => p.Highlights);
        return this;
    }

    public IProfileQueryRepository WithTalents()
    {
        _query = _query
            .Include(p => p.Talents);

        return this;
    }

    public IProfileQueryRepository WithProject()
    {
        _query = _query
            .Include(p => p.Projects);

        return this;
    }

    public IProfileQueryRepository WithTestimonials()
    {
        _query = _query
            .Include(p => p.Testimonials);

        return this;
    }

    public IProfileQueryRepository WhereCondition(Expression<Func<Profile, bool>> expression)
    {
        _whereCondition = expression;
        return this;
    }

    public async Task<Profile?> BuildFirstOrDefaultAsync()
    {
        var finalQuery = _query.AsNoTracking().AsSplitQuery();

        return await finalQuery
            .FirstOrDefaultAsync(_whereCondition)
            .ConfigureAwait(false);
    }
}