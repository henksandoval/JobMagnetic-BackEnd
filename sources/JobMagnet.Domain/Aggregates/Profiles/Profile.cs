using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Aggregates.SkillTypes;
using JobMagnet.Domain.Enums;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Domain.Services;
using JobMagnet.Domain.Shared.Base.Aggregates;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles;

public partial class Profile : SoftDeletableAggregateRoot<ProfileId>
{
    private readonly HashSet<Project> _portfolio = [];
    private readonly HashSet<Talent> _talents = [];
    private readonly HashSet<Testimonial> _testimonials = [];
    private readonly HashSet<VanityUrl> _vanityUrls = [];
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? ProfileImageUrl { get; private set; }
    public DateOnly? BirthDate { get; private set; }
    public string? MiddleName { get; private set; }
    public string? SecondLastName { get; private set; }

    public virtual ProfileHeader? Header { get; private set; }
    public virtual SkillSet? SkillSet { get; private set; }
    public virtual CareerHistory? CareerHistory { get; private set; }
    public TalentShowcase TalentShowcase { get; private set; }
    public virtual IReadOnlyCollection<Talent> Talents => _talents;
    public virtual IReadOnlyCollection<Project> Portfolio => _portfolio;
    public virtual IReadOnlyCollection<Testimonial> Testimonials => _testimonials;
    public virtual IReadOnlyCollection<VanityUrl> VanityUrls => _vanityUrls;
    public bool HaveHeader => Header is not null;
    public bool HaveSkillSet => SkillSet is not null;
    public bool HaveCareerHistory => CareerHistory is not null;

    private Profile(ProfileId id, DateTimeOffset addedAt, DateTimeOffset? lastModifiedAt, DateTimeOffset? deletedAt) :
        base(id, addedAt, lastModifiedAt, deletedAt)
    {
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
}