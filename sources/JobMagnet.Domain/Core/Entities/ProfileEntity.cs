using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Core.Entities.Skills;
using JobMagnet.Domain.Core.Enums;
using JobMagnet.Domain.Services;

namespace JobMagnet.Domain.Core.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class ProfileEntity : SoftDeletableEntity<long>
{
    private readonly HashSet<PublicProfileIdentifierEntity> _publicProfileIdentifiers = [];
    private readonly HashSet<TestimonialEntity> _testimonials = [];
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? MiddleName { get; set; }
    public string? SecondLastName { get; set; }

    public virtual ResumeEntity? Resume { get; set; }
    public virtual SkillSet? SkillSet { get; set; }
    public virtual ServiceEntity? Services { get; set; }
    public virtual SummaryEntity? Summary { get; set; }
    public SocialProof SocialProof { get; }
    public virtual ICollection<TalentEntity> Talents { get; set; } = new HashSet<TalentEntity>();
    public virtual ICollection<PortfolioGalleryEntity> PortfolioGallery { get; set; } =
        new HashSet<PortfolioGalleryEntity>();
    public virtual IReadOnlyCollection<TestimonialEntity> Testimonials => _testimonials;
    public virtual ICollection<PublicProfileIdentifierEntity> PublicProfileIdentifiers => _publicProfileIdentifiers;

    public ProfileEntity()
    {
        SocialProof = new SocialProof(this);
    }

    public void CreateAndAssignPublicIdentifier(IProfileSlugGenerator slugGenerator)
    {
        if (_publicProfileIdentifiers.Any(p => p.Type == LinkType.Primary))
        {
            return;
        }

        var generatedSlug = slugGenerator.GenerateProfileSlug(this);
        AddPublicProfileIdentifier(generatedSlug);
    }

    public void AddPublicProfileIdentifier(string slug, LinkType type = LinkType.Primary)
    {
        Guard.IsNotNullOrEmpty(slug);

        var publicIdentifier = new PublicProfileIdentifierEntity(slug, Id);

        _publicProfileIdentifiers.Add(publicIdentifier);
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

    public void AddTalents(List<TalentEntity> talents)
    {
        ArgumentNullException.ThrowIfNull(talents);

        foreach (var talent in talents)
            Talents.Add(talent);
    }

    public void AddResume(ResumeEntity resume)
    {
        ArgumentNullException.ThrowIfNull(resume);

        Resume = resume;
    }

    public void AddSkill(SkillSet skillSet)
    {
        ArgumentNullException.ThrowIfNull(skillSet);

        SkillSet = skillSet;
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

    internal void AddTestimonial(TestimonialEntity testimonial)
    {
        _testimonials.Add(testimonial);
    }

    public void AddPortfolioItems(List<PortfolioGalleryEntity> portfolio)
    {
        foreach (var gallery in portfolio)
            PortfolioGallery.Add(gallery);
    }
}