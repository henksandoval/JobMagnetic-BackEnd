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
            Resume = p.Resume != null && p.Resume.IsDeleted
                ? null
                : new ResumeEntity
                {
                    Id = p.Resume!.Id,
                    Overview = p.Resume.Overview,
                    ContactInfo = p.Resume.ContactInfo.Select(c => new ContactInfoEntity
                    {
                        Id = c.Id,
                        Value = c.Value,
                        ContactType = new ContactTypeEntity
                        {
                            Id = c.ContactType.Id,
                            Name = c.ContactType.Name
                        }
                    }).ToList()
                },
            Skill = p.Skill,
            Summary = p.Summary,
            Testimonials = p.Testimonials
        });
        return this;
    }

    public IProfileQueryRepository WithSkills()
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
            Skill = p.Skill != null && p.Skill.IsDeleted
                ? null
                : new SkillEntity
                {
                    Id = p.Skill!.Id,
                    Overview = p.Skill.Overview,
                    SkillDetails = p.Skill.SkillDetails.Select(s => new SkillItemEntity
                    {
                        Id = s.Id,
                        Name = s.Name,
                        ProficiencyLevel = s.ProficiencyLevel,
                        Rank = s.Rank,
                        IconUrl = s.IconUrl,
                        Category = s.Category
                    }).ToList()
                },
            Summary = p.Summary,
            Testimonials = p.Testimonials
        });
        return this;
    }

    public IProfileQueryRepository WithServices()
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
            Services = p.Services != null && p.Services.IsDeleted
                ? null
                : new ServiceEntity
                {
                    Id = p.Services!.Id,
                    Overview = p.Services.Overview,
                    GalleryItems = p.Services.GalleryItems.Select(g => new ServiceGalleryItemEntity
                    {
                        Id = g.Id,
                        Title = g.Title,
                        Description = g.Description,
                        UrlImage = g.UrlImage,
                        UrlLink = g.UrlLink,
                        UrlVideo = g.UrlVideo,
                        Type = g.Type
                    }).ToList()
                },
            Talents = p.Talents,
            PortfolioGallery = p.PortfolioGallery,
            Resume = p.Resume,
            Skill = p.Skill,
            Summary = p.Summary,
            Testimonials = p.Testimonials
        });
        return this;
    }

    public IProfileQueryRepository WithSummary()
    {
        _query = _query
            .Include(p => p.Summary)
            .ThenInclude(p => p.Education);
        _query = _query
            .Include(p => p.Summary)
            .ThenInclude(p => p.WorkExperiences);
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
            Skill = p.Skill,
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
            Skill = p.Skill,
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
            Skill = p.Skill,
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