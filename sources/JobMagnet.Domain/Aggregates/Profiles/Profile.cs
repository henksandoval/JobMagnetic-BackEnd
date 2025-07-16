using CommunityToolkit.Diagnostics;
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

    public PersonName Name { get; private set; }
    public ProfileImage ProfileImage { get; private set; }
    public BirthDate BirthDate { get; private set; }

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
    }

    private Profile(ProfileId id, DateTimeOffset addedAt, DateTimeOffset? lastModifiedAt, DateTimeOffset? deletedAt) :
        base(id, addedAt, lastModifiedAt, deletedAt)
    {
    }

    private Profile(
        ProfileId id,
        IClock clock,
        PersonName name,
        ProfileImage profileImage,
        BirthDate birthDate) : base(id, clock)
    {
        Name = name;
        ProfileImage = profileImage;
        BirthDate = birthDate;
    }

    public static Profile CreateInstance(
        IGuidGenerator guidGenerator,
        IClock clock,
        PersonName name,
        BirthDate birthDate,
        ProfileImage profileImage)
    {
        Guard.IsNotNull(name);
        Guard.IsNotNull(birthDate);
        Guard.IsNotNull(profileImage);

        var id = new ProfileId(guidGenerator.NewGuid());

        return new Profile(id, clock, name, profileImage, birthDate);
    }

    public void Update(
        PersonName name,
        BirthDate birthDate,
        ProfileImage profileImage,
        IClock clock)
    {
        Guard.IsNotNull(name);
        Guard.IsNotNull(birthDate);
        Guard.IsNotNull(profileImage);

        Name = name;
        BirthDate = birthDate;
        ProfileImage = profileImage;

        UpdateModificationDetails(clock);
    }

    public void ChangeName(PersonName newName, IClock clock)
    {
        Guard.IsNotNull(newName);
        Name = newName;
        UpdateModificationDetails(clock);
    }

    public void ChangeProfileImage(ProfileImage newImage, IClock clock)
    {
        Guard.IsNotNull(newImage);
        ProfileImage = newImage;
        UpdateModificationDetails(clock);
    }

    public void ChangeBirthDate(BirthDate newBirthDate, IClock clock)
    {
        Guard.IsNotNull(newBirthDate);
        BirthDate = newBirthDate;
        UpdateModificationDetails(clock);
    }
}