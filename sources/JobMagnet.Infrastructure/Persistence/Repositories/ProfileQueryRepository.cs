using System.Linq.Expressions;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Repositories;

public class ProfileQueryRepository(JobMagnetDbContext dbContext)
    : Repository<Profile, ProfileId>(dbContext), IProfileQueryRepository
{
    private IQueryable<Profile> _query = dbContext.Profiles;
    private Expression<Func<Profile, bool>> _whereCondition = x => true;

    public IProfileQueryRepository WithProfileHeader()
    {
        _query = _query
            .Include(p => p.Header)
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

    public IProfileQueryRepository WithCareerHistory()
    {
        _query = _query
            .Include(carer => carer.CareerHistory)
            .ThenInclude(career => career.AcademicDegree)
            .Include(profile => profile.CareerHistory)
            .ThenInclude(career => career.WorkExperiences)
            .ThenInclude(work => work.Highlights);

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
            .Include(p => p.Portfolio);

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

    public async Task<Profile?> BuildFirstOrDefaultAsync(CancellationToken cancellationToken, bool asNoTracking)
    {
        if (asNoTracking)
            _query = _query.AsNoTracking();

        var finalQuery = _query.AsSplitQuery();

        return await finalQuery
            .FirstOrDefaultAsync(_whereCondition, cancellationToken)
            .ConfigureAwait(false);
    }
}