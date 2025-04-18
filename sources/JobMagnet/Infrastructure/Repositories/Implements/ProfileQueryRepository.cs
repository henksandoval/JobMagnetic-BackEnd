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

    public async Task<ProfileEntity?> GetFirstByExpressionWithIncludesAsync(Expression<Func<ProfileEntity, bool>> expression)
    {
        return await _query.FirstOrDefaultAsync(expression).ConfigureAwait(false);
    }

    public IProfileQueryRepository IncludeTalents()
    {
        _query = _query.Include(p => p.Talents);
        return this;
    }
    
    public IProfileQueryRepository IncludeTestimonials()
    {
        _query = _query
            .Include(p => p.Testimonials);
        return this;
    }

    public IProfileQueryRepository IncludeResume()
    {
        _query = _query
            .Include(p => p.Resume)
            .ThenInclude(p => p.ContactInfo)
            .ThenInclude(p => p.ContactType);

        return this;
    }

    public IProfileQueryRepository IncludeSkill()
    {
        _query = _query
            .Include(p => p.Skill)
            .ThenInclude(p => p.SkillDetails);

        return this;
    }

    public IProfileQueryRepository IncludePortfolioGallery()
    {
        _query = _query
            .Include(p => p.PortfolioGallery);

        return this;
    }
}