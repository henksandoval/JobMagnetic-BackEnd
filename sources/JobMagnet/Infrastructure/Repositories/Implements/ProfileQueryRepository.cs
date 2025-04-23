using System.Linq.Expressions;
using JobMagnet.Infrastructure.Context;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Repositories.Implements;

public class ProfileQueryRepository(JobMagnetDbContext dbContext, IDbContextFactory dbContextFactory)
    : Repository<ProfileEntity, long>(dbContext), IProfileQueryRepository
{
    private readonly JobMagnetDbContext _dbContext = dbContext;
    private readonly List<Func<JobMagnetDbContext, ProfileEntity, Task>> _relationLoaders = [];
    private Expression<Func<ProfileEntity, bool>> _expression = x => true;

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

        var loaders = _relationLoaders.Select(func =>
        {
            var newDbContext = dbContextFactory.CreateDbContext();
            return func(newDbContext, profile);
        });

        await Task.WhenAll(loaders).ConfigureAwait(false);

        return profile;
    }

    public IProfileQueryRepository WithResume()
    {
        _relationLoaders.Add(async (contextFactory, profile) =>
        {
            contextFactory.Attach(profile);

            var loadResume = contextFactory.Entry(profile)
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
        _relationLoaders.Add(async (contextFactory, profile) =>
        {
            contextFactory.Attach(profile);

            var loadSkills = contextFactory.Entry(profile)
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
        _relationLoaders.Add(async (contextFactory, profile) =>
        {
            var loadTalents = contextFactory.Entry(profile)
                .Collection(p => p.Talents)
                .LoadAsync();

            await loadTalents;
        });

        return this;
    }

    public IProfileQueryRepository WithPortfolioGallery()
    {
        _relationLoaders.Add(async (contextFactory, profile) =>
        {
            var loadPortfolioGallery = contextFactory.Entry(profile)
                .Collection(p => p.PortfolioGallery)
                .LoadAsync();

            await loadPortfolioGallery;
        });

        return this;
    }

    public IProfileQueryRepository WithSummaries()
    {
        _relationLoaders.Add(async (contextFactory, profile) =>
        {
            var loadSummaries = contextFactory.Entry(profile)
                .Collection(p => p.Summaries)
                .LoadAsync();

            await loadSummaries;
        });

        return this;
    }

    public IProfileQueryRepository WithServices()
    {
        _relationLoaders.Add(async (contextFactory, profile) =>
        {
            var loadServices = contextFactory.Entry(profile)
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
        _relationLoaders.Add(async (contextFactory, profile) =>
        {
            var loadTestimonials = contextFactory.Entry(profile)
                .Collection(p => p.Testimonials)
                .LoadAsync();

            await loadTestimonials;
        });

        return this;
    }
}