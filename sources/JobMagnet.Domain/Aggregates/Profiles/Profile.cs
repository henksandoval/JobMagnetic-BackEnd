using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Shared.Base.Aggregates;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles;

public partial class Profile : SoftDeletableAggregateRoot<ProfileId>
{
    private readonly HashSet<Project> _portfolio = [];
    private readonly HashSet<Talent> _talentShowcase = [];
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
    public virtual IReadOnlyCollection<Talent> TalentShowcase => _talentShowcase;
    public virtual IReadOnlyCollection<Project> Portfolio => _portfolio;
    public virtual IReadOnlyCollection<Testimonial> Testimonials => _testimonials;
    public virtual IReadOnlyCollection<VanityUrl> VanityUrls => _vanityUrls;
    public bool HaveHeader => Header is not null;
    public bool HaveSkillSet => SkillSet is not null;
    public bool HaveCareerHistory => CareerHistory is not null;

    private Profile() : base()
    {
        TalentShowcase = new TalentShowcase(this);
    }

    private Profile(ProfileId id, DateTimeOffset addedAt, DateTimeOffset? lastModifiedAt, DateTimeOffset? deletedAt) :
        base(id, addedAt, lastModifiedAt, deletedAt)
    {
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


    #region VanityUrls

    public void AssignDefaultVanityUrl(IGuidGenerator guidGenerator, IProfileSlugGenerator slugGenerator)
    {
        ExecuteAssignDefaultVanityUrl(guidGenerator, slugGenerator);
    }

    public void AddVanityUrl(IGuidGenerator guidGenerator, string slug, LinkType type = LinkType.Primary)
    {
        ExecuteAddVanityUrl(guidGenerator, slug, type);
    }

    #endregion

    #region Skills

    public void AddSkillSet(SkillSet skillSet)
    {
        Guard.IsNotNull(skillSet);

        SkillSet = skillSet;
    }

    public void UpdateSkillSet(string overview)
    {
        if (!HaveSkillSet)
            throw new JobMagnetDomainException($"The profile {Id} does not have skills set.");

        SkillSet!.Update(overview);
    }

    public void AddSkill(IGuidGenerator guidGenerator, ushort proficiencyLevel, SkillType skillType)
    {
        if (!HaveSkillSet)
            throw new JobMagnetDomainException($"The profile {Id} does not have skills set.");

        SkillSet!.AddSkill(guidGenerator, proficiencyLevel, skillType);
    }

    public void UpdateSkill(SkillId skillId, ushort skillProficiencyLevel)
    {
        if (!HaveSkillSet)
            throw new JobMagnetDomainException($"The profile {Id} does not have skills set.");

        SkillSet!.UpdateSkill(skillId, skillProficiencyLevel);
    }

    public void RemoveSkill(SkillId skillId)
    {
        if (!HaveSkillSet)
            throw new JobMagnetDomainException($"The profile {Id} does not have skills set.");

        SkillSet!.RemoveSkill(skillId);
    }

    public IReadOnlyCollection<Skill> GetSkills()
    {
        if (!HaveSkillSet)
            throw new JobMagnetDomainException($"The profile {Id} does not have skills set.");

        return SkillSet!.Skills;
    }

    public void ArrangeSkills(IEnumerable<SkillId> orderedSkills)
    {
        if (!HaveSkillSet)
            throw new JobMagnetDomainException($"The profile {Id} does not have skills set.");

        SkillSet!.ArrangeSkills(orderedSkills);
    }

    #endregion
}