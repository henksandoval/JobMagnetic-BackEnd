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
    private Expression<Func<ProfileEntity, bool>> _whereCondition = x => true;
    private readonly List<Expression<Func<ProfileEntity, ProfileEntity>>> _projections = [];

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
            Talents = p.Talents.Select(t => new TalentEntity
            {
                Id = t.Id,
                Description = t.Description
            }).ToList(),
            PortfolioGallery = p.PortfolioGallery,
            Resume = p.Resume,
            Skill = p.Skill,
            Summaries = p.Summaries,
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
            Services = new ServiceEntity
            {
                Id = p.Services.Id,
                Overview = p.Services.Overview,
                GalleryItems = p.Services.GalleryItems.Select(g => new ServiceGalleryItemEntity
                {
                    Id = g.Id,
                    Title = g.Title
                }).ToList()
            },
            Talents = p.Talents,
            PortfolioGallery = p.PortfolioGallery,
            Resume = p.Resume,
            Skill = p.Skill,
            Summaries = p.Summaries,
            Testimonials = p.Testimonials
        });
        return this;
    }

    public IProfileQueryRepository WithSummaries()
    {
        _query = _query.Include(p => p.Summaries);
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
            Summaries = p.Summaries,
            Testimonials = p.Testimonials.Select(t => new TestimonialEntity
            {
                Id = t.Id,
                Name = t.Name,
                JobTitle = t.JobTitle,
                Feedback = t.Feedback
            }).ToList()
        });
        return this;
    }

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
            Resume = new ResumeEntity
            {
                Id = p.Resume.Id,
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
                }).ToList(),
            },
            Skill = p.Skill,
            Summaries = p.Summaries,
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
            Skill = new SkillEntity
            {
                Id = p.Skill.Id,
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
            Summaries = p.Summaries,
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
            PortfolioGallery = p.PortfolioGallery.Select(pg => new PortfolioGalleryEntity
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
            Summaries = p.Summaries,
            Testimonials = p.Testimonials
        });
        return this;
    }

    public IProfileQueryRepository WhereCondition(Expression<Func<ProfileEntity, bool>> expression)
    {
        _whereCondition = expression;
        return this;
    }

    public IProfileQueryRepository SelectFields(Expression<Func<ProfileEntity, ProfileEntity>> selectExpression)
    {
        _query = _query.Select(selectExpression);
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