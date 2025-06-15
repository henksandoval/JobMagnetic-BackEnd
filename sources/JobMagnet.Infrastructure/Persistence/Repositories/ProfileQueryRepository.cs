using System.Linq.Expressions;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Repositories;

public class ProfileQueryRepository(JobMagnetDbContext dbContext)
    : Repository<ProfileEntity, long>(dbContext), IProfileQueryRepository
{
    private readonly List<Expression<Func<ProfileEntity, ProfileEntity>>> _projections = [];
    private IQueryable<ProfileEntity> _query = dbContext.Profiles;
    private Expression<Func<ProfileEntity, bool>> _whereCondition = x => true;

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

    public IProfileQueryRepository WithServices()
    {
        _query = _query
            .Include(p => p.Services)
            .ThenInclude(s => s.GalleryItems);

        return this;
    }

    public IProfileQueryRepository WithSummary()
    {
        _query = _query
            .Include(p => p.Summary)
            .ThenInclude(p => p.Education);
        _query = _query
            .Include(p => p.Summary)
            .ThenInclude(p => p.WorkExperiences)
            .ThenInclude(p => p.Responsibilities);
        return this;
    }

    public IProfileQueryRepository WithTalents()
    {
        _projections.Add(p => new ProfileEntity
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            MiddleName = p.MiddleName,
            SecondLastName = p.SecondLastName,
            ProfileImageUrl = p.ProfileImageUrl,
            BirthDate = p.BirthDate,
            Services = p.Services,
            Talents = p.Talents.Where(t => !t.IsDeleted).Select(t => new TalentEntity
            {
                Id = t.Id,
                Description = t.Description
            }).ToList(),
            PortfolioGallery = p.PortfolioGallery,
            Resume = p.Resume,
            SkillSet = p.SkillSet,
            Summary = p.Summary,
            Testimonials = p.Testimonials
        });
        return this;
    }

    public IProfileQueryRepository WithPortfolioGallery()
    {
        _projections.Add(p => new ProfileEntity
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            MiddleName = p.MiddleName,
            SecondLastName = p.SecondLastName,
            ProfileImageUrl = p.ProfileImageUrl,
            BirthDate = p.BirthDate,
            Services = p.Services,
            Talents = p.Talents,
            PortfolioGallery = p.PortfolioGallery.Where(portfolio => !portfolio.IsDeleted).Select(pg =>
                new PortfolioGalleryEntity
                {
                    Id = pg.Id,
                    Position = pg.Position,
                    Title = pg.Title,
                    Description = pg.Description,
                    UrlImage = pg.UrlImage,
                    UrlLink = pg.UrlLink,
                    UrlVideo = pg.UrlVideo,
                    Type = pg.Type
                }).ToList(),
            Resume = p.Resume,
            SkillSet = p.SkillSet,
            Summary = p.Summary,
            Testimonials = p.Testimonials
        });
        return this;
    }

    public IProfileQueryRepository WithTestimonials()
    {
        _projections.Add(p => new ProfileEntity
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            MiddleName = p.MiddleName,
            SecondLastName = p.SecondLastName,
            ProfileImageUrl = p.ProfileImageUrl,
            BirthDate = p.BirthDate,
            Services = p.Services,
            Talents = p.Talents,
            PortfolioGallery = p.PortfolioGallery,
            Resume = p.Resume,
            SkillSet = p.SkillSet,
            Summary = p.Summary,
            Testimonials = p.Testimonials.Where(t => !t.IsDeleted).Select(t => new TestimonialEntity
            {
                Id = t.Id,
                Name = t.Name,
                JobTitle = t.JobTitle,
                Feedback = t.Feedback,
                PhotoUrl = t.PhotoUrl
            }).ToList()
        });
        return this;
    }

    public IProfileQueryRepository WhereCondition(Expression<Func<ProfileEntity, bool>> expression)
    {
        _whereCondition = expression;
        return this;
    }

    public async Task<ProfileEntity?> BuildFirstOrDefaultAsync()
    {
        var finalQuery = _query.AsNoTracking().AsSplitQuery();

        finalQuery = _projections.Aggregate(finalQuery, (current, projection) => current.Select(projection));

        return await finalQuery
            .FirstOrDefaultAsync(_whereCondition)
            .ConfigureAwait(false);
    }
}