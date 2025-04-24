using System.Linq.Expressions;
using JobMagnet.Infrastructure.Context;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Repositories.Implements;

public class ProfileQueryRepository(JobMagnetDbContext dbContext)
    : Repository<ProfileEntity, long>(dbContext), IProfileQueryRepository
{
    private IQueryable<ProfileEntity> _query = dbContext.Set<ProfileEntity>();
    private Expression<Func<ProfileEntity, bool>> _expression = x => true;

    public IProfileQueryRepository WithTalents()
    {
        _query = _query.Include(p => p.Talents);
        return this;
    }

    public IProfileQueryRepository WithSummaries()
    {
        _query = _query.Include(p => p.Summaries);
        return this;
    }

    public IProfileQueryRepository WithServices()
    {
        _query = _query
            .Include(p => p.Services)
            .ThenInclude(p => p.GalleryItems);
        return this;
    }

    public IProfileQueryRepository WithTestimonials()
    {
        _query = _query
            .Include(p => p.Testimonials);
        return this;
    }

    public IProfileQueryRepository WithResume()
    {
        _query = _query
            .Include(p => p.Resume)
            .ThenInclude(p => p.ContactInfo)
            .ThenInclude(p => p.ContactType);

        return this;
    }

    public IProfileQueryRepository WithSkills()
    {
        _query = _query
            .Include(p => p.Skill)
            .ThenInclude(p => p.SkillDetails);

        return this;
    }

    public IProfileQueryRepository WithPortfolioGallery()
    {
        _query = _query
            .Include(p => p.PortfolioGallery);

        return this;
    }

    public IProfileQueryRepository WhereCondition(Expression<Func<ProfileEntity, bool>> expression)
    {
        _expression = expression;
        return this;
    }

    public async Task<ProfileEntity?> BuildAsync()
    {
        var profile = await _query
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(_expression)
            .ConfigureAwait(false);

        return profile;
    }
}