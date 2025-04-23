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
    private Expression<Func<ProfileEntity, bool>> _expression = x => true;
    private readonly List<Func<ProfileEntity, Task>> _relationLoaders = [];

    public IProfileQueryRepository WhereCondition(Expression<Func<ProfileEntity, bool>> expression)
    {
        _expression = expression;
        return this;
    }

    public async Task<ProfileEntity?> BuildAsync()
    {
        var profile = await _dbContext.Set<ProfileEntity>()
            .Where(_expression)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

        if (profile == null) return null;

        foreach (var loader in _relationLoaders)
        {
            await loader(profile);
        }

        return profile;
    }

    public IProfileQueryRepository WithResume()
    {
        _relationLoaders.Add(async profile =>
        {
            var loadResume = _dbContext.Entry(profile)
                .Reference(p => p.Resume)
                .Query()
                .Include(r => r.ContactInfo)
                .ThenInclude(ci => ci.ContactType)
                .LoadAsync();

            await loadResume;
        });

        return this;
    }

    public IProfileQueryRepository WithSkills()
    {
        _relationLoaders.Add(async profile =>
        {
            var loadSkills = _dbContext.Entry(profile)
                .Reference(p => p.Skill)
                .Query()
                .Include(s => s.SkillDetails)
                .LoadAsync();

            await loadSkills;
        });

        return this;
    }

    public IProfileQueryRepository WithTalents()
    {
        _relationLoaders.Add(async profile =>
        {
            var loadTalents = _dbContext.Entry(profile)
                .Collection(p => p.Talents)
                .LoadAsync();

            await loadTalents;
        });

        return this;
    }

    public IProfileQueryRepository WithPortfolioGallery()
    {
        _relationLoaders.Add(async profile =>
        {
            var loadPortfolioGallery = _dbContext.Entry(profile)
                .Collection(p => p.PortfolioGallery)
                .LoadAsync();

            await loadPortfolioGallery;
        });

        return this;
    }

    public IProfileQueryRepository WithSummaries()
    {
        _relationLoaders.Add(async profile =>
        {
            var loadSummaries = _dbContext.Entry(profile)
                .Collection(p => p.Summaries)
                .LoadAsync();

            await loadSummaries;
        });

        return this;
    }

    public IProfileQueryRepository WithServices()
    {
        _relationLoaders.Add(async profile =>
        {
            var loadServices = _dbContext.Entry(profile)
                .Collection(p => p.Services)
                .Query()
                .Include(s => s.GalleryItems)
                .LoadAsync();

            await loadServices;
        });

        return this;
    }

    public IProfileQueryRepository WithTestimonials()
    {
        _relationLoaders.Add(async profile =>
        {
            var loadTestimonials = _dbContext.Entry(profile)
                .Collection(p => p.Testimonials)
                .LoadAsync();

            await loadTestimonials;
        });

        return this;
    }
}