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
    private readonly JobMagnetDbContext _dbContext = dbContext;
    private IQueryable<ProfileEntity> _query = dbContext.Set<ProfileEntity>();

    public async Task<ProfileEntity?> GetFirstByExpressionWithIncludesAsync(Expression<Func<ProfileEntity, bool>> expression)
    {
        var profile = await _query
            .Where(expression)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

        if (profile == null) return null;

        await _dbContext.Entry(profile)
            .Reference(p => p.Resume)
            .Query()
            .Include(r => r.ContactInfo)
            .ThenInclude(ci => ci.ContactType)
            .LoadAsync();

        await _dbContext.Entry(profile)
            .Reference(p => p.Skill)
            .Query()
            .Include(s => s.SkillDetails)
            .LoadAsync();

        await _dbContext.Entry(profile)
            .Collection(p => p.Talents)
            .LoadAsync();

        await _dbContext.Entry(profile)
            .Collection(p => p.PortfolioGallery)
            .LoadAsync();

        await _dbContext.Entry(profile)
            .Collection(p => p.Services)
            .Query()
            .LoadAsync();

        await _dbContext.Entry(profile)
            .Collection(p => p.Testimonials)
            .LoadAsync();

        var data = profile;
        return data;
    }

    public IProfileQueryRepository IncludeTalents()
    {
        _query = _query.Include(p => p.Talents);
        return this;
    }
    public IProfileQueryRepository IncludeService()
    {
        _query = _query
            .Include(p => p.Services)
            .ThenInclude(p => p.GalleryItems);
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