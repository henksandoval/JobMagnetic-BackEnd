using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Core.Entities.Skills;

namespace JobMagnet.Domain.Core.Entities;

public class ProfileEntity : SoftDeletableEntity<long>
{
    private readonly HashSet<PublicProfileIdentifierEntity> _publicProfileIdentifiers = [];
    private readonly HashSet<TalentEntity> _talents = [];
    private readonly HashSet<TestimonialEntity> _testimonials = [];
    private readonly HashSet<Project> _projects = [];
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? MiddleName { get; set; }
    public string? SecondLastName { get; set; }

    public virtual ResumeEntity? Resume { get; private set; }
    public virtual SkillSet? SkillSet { get; private set; }
    public virtual SummaryEntity? Summary { get; set; }
    public SocialProof SocialProof { get; }
    public Portfolio Portfolio { get; }
    public VanityUrls VanityUrls { get; }
    public TalentShowcase TalentShowcase { get; }
    public virtual IReadOnlyCollection<TalentEntity> Talents => _talents;
    public virtual IReadOnlyCollection<Project> Projects => _projects;
    public virtual IReadOnlyCollection<TestimonialEntity> Testimonials => _testimonials;
    public virtual IReadOnlyCollection<PublicProfileIdentifierEntity> PublicProfileIdentifiers => _publicProfileIdentifiers;

    public ProfileEntity()
    {
        SocialProof = new SocialProof(this);
        Portfolio = new Portfolio(this);
        VanityUrls = new VanityUrls(this);
        TalentShowcase = new TalentShowcase(this);
    }

    public void AddTalent(string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        
        var talentEntity = new TalentEntity(description, this.Id);
        _talents.Add(talentEntity);
    }
    
    public void AddSkill(SkillSet skillSet)
    {
        Guard.IsNotNull(skillSet);

        SkillSet = skillSet;
    }

    public void AddSummary(SummaryEntity summary)
    {
        Guard.IsNotNull(summary);

        Summary = summary;
    }

    public void AddResume(ResumeEntity resume)
    {
        Guard.IsNotNull(resume);

        Resume = resume;
    }

    internal void AddTestimonial(TestimonialEntity testimonial)
    {
        _testimonials.Add(testimonial);
    }

    internal void AddProjectToPortfolio(Project project)
    {
        _projects.Add(project);
    }

    internal void AddPublicProfileIdentifier(PublicProfileIdentifierEntity publicProfile)
    {
        _publicProfileIdentifiers.Add(publicProfile);
    }
}