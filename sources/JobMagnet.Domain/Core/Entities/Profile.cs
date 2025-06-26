using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Core.Entities.Skills;

namespace JobMagnet.Domain.Core.Entities;

public class Profile : SoftDeletableEntity<long>
{
    private readonly HashSet<Project> _projects = [];
    private readonly HashSet<VanityUrl> _vanityUrls = [];
    private readonly HashSet<Talent> _talents = [];
    private readonly HashSet<Testimonial> _testimonials = [];
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? ProfileImageUrl { get; private set; }
    public DateOnly? BirthDate { get; private set; }
    public string? MiddleName { get; private set; }
    public string? SecondLastName { get; private set; }

    public virtual Resume? Resume { get; private set; }
    public virtual SkillSet? SkillSet { get; private set; }
    public virtual ProfessionalSummary? ProfessionalSummary { get; private set; }
    public SocialProof SocialProof { get; private set; }
    public Portfolio Portfolio { get; private set; }
    public VanityUrls LinkManager { get; private set; }
    public TalentShowcase TalentShowcase { get; private set; }
    public virtual IReadOnlyCollection<Talent> Talents => _talents;
    public virtual IReadOnlyCollection<Project> Projects => _projects;
    public virtual IReadOnlyCollection<Testimonial> Testimonials => _testimonials;
    public virtual IReadOnlyCollection<VanityUrl> VanityUrls => _vanityUrls;

    private Profile()
    {
        InitializeEncapsulatedCollector();
    }

    [SetsRequiredMembers]
    public Profile(
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

    public void AddTalent(Talent talent)
    {
        _talents.Add(talent);
    }

    public void AddSkill(SkillSet skillSet)
    {
        Guard.IsNotNull(skillSet);

        SkillSet = skillSet;
    }

    public void AddSummary(ProfessionalSummary professionalSummary)
    {
        Guard.IsNotNull(professionalSummary);

        ProfessionalSummary = professionalSummary;
    }

    public void AddResume(Resume resume)
    {
        Guard.IsNotNull(resume);

        Resume = resume;
    }

    internal void AddTestimonial(Testimonial testimonial)
    {
        _testimonials.Add(testimonial);
    }

    internal void AddProjectToPortfolio(Project project)
    {
        _projects.Add(project);
    }

    internal void AddPublicProfileIdentifier(VanityUrl publicProfile)
    {
        _vanityUrls.Add(publicProfile);
    }

    private void InitializeEncapsulatedCollector()
    {
        SocialProof = new SocialProof(this);
        Portfolio = new Portfolio(this);
        LinkManager = new VanityUrls(this);
        TalentShowcase = new TalentShowcase(this);
    }
}