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
    private readonly HashSet<PortfolioGalleryEntity> _portfolio = [];
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
    public Portfolio Portfolio { get; }
    public VanityUrls VanityUrls { get; }
    public virtual ICollection<TalentEntity> Talents { get; set; } = new HashSet<TalentEntity>();
    public virtual IReadOnlyCollection<PortfolioGalleryEntity> PortfolioGallery => _portfolio;
    public virtual IReadOnlyCollection<TestimonialEntity> Testimonials => _testimonials;
    public virtual IReadOnlyCollection<PublicProfileIdentifierEntity> PublicProfileIdentifiers => _publicProfileIdentifiers;

    public ProfileEntity()
    {
        SocialProof = new SocialProof(this);
        Portfolio = new Portfolio(this);
        VanityUrls = new VanityUrls(this);
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

    internal void AddTestimonial(TestimonialEntity testimonial)
    {
        _testimonials.Add(testimonial);
    }

    internal void AddPortfolio(PortfolioGalleryEntity portfolio)
    {
        _portfolio.Add(portfolio);
    }

    internal void AddPublicProfileIdentifier(PublicProfileIdentifierEntity publicProfile)
    {
        _publicProfileIdentifiers.Add(publicProfile);
    }
}