using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Aggregates.Skills;
using JobMagnet.Domain.Shared.Base;
using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles;

public readonly record struct ProfileId(Guid Value) : IStronglyTypedId<ProfileId>;

public class Profile : SoftDeletableAggregate<ProfileId>
{
    private readonly HashSet<Project> _projects = [];
    private readonly HashSet<Talent> _talents = [];
    private readonly HashSet<Testimonial> _testimonials = [];
    private readonly HashSet<VanityUrl> _vanityUrls = [];
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? ProfileImageUrl { get; private set; }
    public DateOnly? BirthDate { get; private set; }
    public string? MiddleName { get; private set; }
    public string? SecondLastName { get; private set; }

    public virtual ProfileHeader? ProfileHeader { get; private set; }
    public virtual SkillSet? SkillSet { get; private set; }
    public virtual CareerHistory? CareerHistory { get; private set; }
    public SocialProof SocialProof { get; private set; }
    public Portfolio Portfolio { get; private set; }
    public VanityUrlManager LinkManager { get; private set; }
    public TalentShowcase TalentShowcase { get; private set; }
    public virtual IReadOnlyCollection<Talent> Talents => _talents;
    public virtual IReadOnlyCollection<Project> Projects => _projects;
    public virtual IReadOnlyCollection<Testimonial> Testimonials => _testimonials;
    public virtual IReadOnlyCollection<VanityUrl> VanityUrls => _vanityUrls;
    public bool HaveSkillSet => SkillSet is not null;

    private Profile(ProfileId id, DateTimeOffset addedAt, DateTimeOffset? lastModifiedAt, DateTimeOffset? deletedAt) :
        base(id, addedAt, lastModifiedAt, deletedAt)
    {
        SocialProof = new SocialProof(this);
        Portfolio = new Portfolio(this);
        LinkManager = new VanityUrlManager(this);
        TalentShowcase = new TalentShowcase(this);
    }

    private Profile(
        ProfileId id,
        IClock clock,
        string? firstName,
        string? lastName,
        string? profileImageUrl = null,
        DateOnly? birthDate = null,
        string? middleName = null,
        string? secondLastName = null) : base(id, clock)
    {
        FirstName = firstName;
        LastName = lastName;
        ProfileImageUrl = profileImageUrl;
        BirthDate = birthDate;
        MiddleName = middleName;
        SecondLastName = secondLastName;

        SocialProof = new SocialProof(this);
        Portfolio = new Portfolio(this);
        LinkManager = new VanityUrlManager(this);
        TalentShowcase = new TalentShowcase(this);
    }

    public static Profile CreateInstance(IGuidGenerator guidGenerator, IClock clock, string? firstName, string? lastName,
        string? profileImageUrl = null, DateOnly? birthDate = null, string? middleName = null, string? secondLastName = null)
    {
        var id = new ProfileId(guidGenerator.NewGuid());

        return new Profile(id, clock, firstName, lastName, profileImageUrl, birthDate, middleName, secondLastName);
    }

    public void Update(
        string? firstName,
        string? lastName,
        string? middleName,
        string? secondLastName,
        DateOnly? birthDate,
        string? profileImageUrl)
    {
        ChangeFirstName(firstName);
        ChangeLastName(lastName);
        ChangeMiddleName(middleName);
        ChangeSecondLastName(secondLastName);
        ChangeBirthDate(birthDate);
        ChangeProfileImageUrl(profileImageUrl);
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

    public void AddSkillSet(SkillSet skillSet)
    {
        Guard.IsNotNull(skillSet);

        SkillSet = skillSet;
    }

    public void AddSummary(CareerHistory careerHistory)
    {
        Guard.IsNotNull(careerHistory);

        CareerHistory = careerHistory;
    }

    public void AddResume(ProfileHeader profileHeader)
    {
        Guard.IsNotNull(profileHeader);

        ProfileHeader = profileHeader;
    }

    internal void AddTestimonial(Testimonial testimonial)
    {
        _testimonials.Add(testimonial);
    }

    internal void AddProjectToPortfolio(Project project)
    {
        _projects.Add(project);
    }

    internal void RemoveProjectToPortfolio(Project project)
    {
        _projects.Remove(project);
    }


    internal void AddPublicProfileIdentifier(VanityUrl publicProfile)
    {
        _vanityUrls.Add(publicProfile);
    }

    internal void AddSkillToSkillSet(Skill skill)
    {

    }
}