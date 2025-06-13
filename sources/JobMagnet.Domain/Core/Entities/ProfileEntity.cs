using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Core.Enums;
using JobMagnet.Domain.Services;

namespace JobMagnet.Domain.Core.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class ProfileEntity : SoftDeletableEntity<long>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? MiddleName { get; set; }
    public string? SecondLastName { get; set; }

    public virtual ResumeEntity? Resume { get; set; }
    public virtual SkillSetEntity? Skill { get; set; }
    public virtual ServiceEntity? Services { get; set; }
    public virtual SummaryEntity? Summary { get; set; }
    public virtual ICollection<TalentEntity> Talents { get; set; } = new HashSet<TalentEntity>();
    public virtual ICollection<PortfolioGalleryEntity> PortfolioGallery { get; set; } =
        new HashSet<PortfolioGalleryEntity>();
    public virtual ICollection<TestimonialEntity> Testimonials { get; set; } = new HashSet<TestimonialEntity>();
    public virtual ICollection<PublicProfileIdentifierEntity> PublicProfileIdentifiers { get; set; } =
        new HashSet<PublicProfileIdentifierEntity>();

    public void CreateAndAssignPublicIdentifier(IProfileSlugGenerator slugGenerator)
    {
        if (this.PublicProfileIdentifiers.Any(p => p.Type == LinkType.Primary))
        {
            return;
        }

        var generatedSlug = slugGenerator.GenerateProfileSlug(this);

        var publicIdentifier = new PublicProfileIdentifierEntity(this, generatedSlug);

        this.PublicProfileIdentifiers.Add(publicIdentifier);
    }

    public void AddPublicProfileIdentifier(PublicProfileIdentifierEntity publicIdentifierEntity)
    {
        ArgumentNullException.ThrowIfNull(publicIdentifierEntity);

        PublicProfileIdentifiers.Add(publicIdentifierEntity);
    }

    public void AddTalent(string talent)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(talent);

        var talentEntity = new TalentEntity
        {
            Id = 0,
            ProfileId = Id,
            Description = talent
        };

        Talents.Add(talentEntity);
    }

    public void AddResume(ResumeEntity resume)
    {
        ArgumentNullException.ThrowIfNull(resume);

        Resume = resume;
    }

    public void AddSkill(SkillSetEntity skillSet)
    {
        ArgumentNullException.ThrowIfNull(skillSet);

        Skill = skillSet;
    }

    public void AddSummary(SummaryEntity summary)
    {
        ArgumentNullException.ThrowIfNull(summary);

        Summary = summary;
    }

    public void AddService(ServiceEntity service)
    {
        ArgumentNullException.ThrowIfNull(service);

        Services = service;
    }

    public void AddPortfolioItem(PortfolioGalleryEntity item)
    {
        ArgumentNullException.ThrowIfNull(item);

        PortfolioGallery.Add(item);
    }

    public void AddTestimonial(TestimonialEntity testimonial)
    {
        ArgumentNullException.ThrowIfNull(testimonial);

        Testimonials.Add(testimonial);
    }
}