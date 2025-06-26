using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Core.Entities.Skills;

namespace JobMagnet.Domain.Core.Entities;

public class ProfileEntity : SoftDeletableEntity<long>
{
    private readonly HashSet<Project> _projects = [];
    private readonly HashSet<PublicProfileIdentifierEntity> _publicProfileIdentifiers = [];
    private readonly HashSet<TalentEntity> _talents = [];
    private readonly HashSet<TestimonialEntity> _testimonials = [];
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? ProfileImageUrl { get; private set; }
    public DateOnly? BirthDate { get; private set; }
    public string? MiddleName { get; private set; }
    public string? SecondLastName { get; private set; }

    public virtual ResumeEntity? Resume { get; private set; }
    public virtual SkillSet? SkillSet { get; private set; }
    public virtual SummaryEntity? Summary { get; private set; }
    public SocialProof SocialProof { get; private set; }
    public Portfolio Portfolio { get; private set; }
    public VanityUrls VanityUrls { get; private set; }
    public TalentShowcase TalentShowcase { get; private set; }
    public virtual IReadOnlyCollection<TalentEntity> Talents => _talents;
    public virtual IReadOnlyCollection<Project> Projects => _projects;
    public virtual IReadOnlyCollection<TestimonialEntity> Testimonials => _testimonials;
    public virtual IReadOnlyCollection<PublicProfileIdentifierEntity> PublicProfileIdentifiers => _publicProfileIdentifiers;

    private ProfileEntity()
    {
        InitializeEncapsulatedCollector();
    }

    [SetsRequiredMembers]
    public ProfileEntity(
        string? firstName,
        string? lastName,
        string? profileImageUrl = null,
        DateOnly? birthDate = null,
        string? middleName = null,
        string? secondLastName = null)
    {
        FirstName = firstName;
        LastName = lastName;
        ProfileImageUrl = profileImageUrl;
        BirthDate = birthDate;
        MiddleName = middleName;
        SecondLastName = secondLastName;

        InitializeEncapsulatedCollector();
    }

    public void ChangeFirstName(string? firstName)
    {
        FirstName = firstName;
    }

    public void ChangeLastName(string? lastName)
    {
        LastName = lastName;
    }

    public void ChangeProfileImageUrl(string? profileImageUrl)
    {
        ProfileImageUrl = profileImageUrl;
    }

    public void ChangeBirthDate(DateOnly? birthDate)
    {
        BirthDate = birthDate;
    }

    public void ChangeMiddleName(string? middleName)
    {
        MiddleName = middleName;
    }

    public void ChangeSecondLastName(string? secondLastName)
    {
        SecondLastName = secondLastName;
    }

    public void AddTalent(TalentEntity talentEntity)
    {
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

    private void InitializeEncapsulatedCollector()
    {
        SocialProof = new SocialProof(this);
        Portfolio = new Portfolio(this);
        VanityUrls = new VanityUrls(this);
        TalentShowcase = new TalentShowcase(this);
    }
}