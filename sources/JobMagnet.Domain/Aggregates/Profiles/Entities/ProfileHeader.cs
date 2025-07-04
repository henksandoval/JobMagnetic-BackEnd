using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Shared.Base.Aggregates;
using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct ProfileHeaderId(Guid Value) : IStronglyTypedId<ProfileHeaderId>;

public class ProfileHeader : SoftDeletableAggregateRoot<ProfileHeaderId>
{
    public const int MaxJobTitleLength = 100;

    private readonly HashSet<ContactInfo> _contactInfo = [];

    public string Title { get; private set; }
    public string JobTitle { get; private set; }
    public string About { get; private set; }
    public string Summary { get; private set; }
    public string Overview { get; private set; }
    public string Suffix { get; private set; }
    public string Address { get; private set; }
    public ProfileId ProfileId { get; private set; }

    public virtual IReadOnlyCollection<ContactInfo>? ContactInfo => _contactInfo;

    private ProfileHeader() : base() { }

    private ProfileHeader(
        ProfileHeaderId id,
        ProfileId profileId,
        IClock clock,
        string title,
        string suffix,
        string jobTitle,
        string about,
        string summary,
        string overview,
        string address) : base(id, clock)
    {
        Guard.HasSizeLessThanOrEqualTo(jobTitle, MaxJobTitleLength);

        Id = id;
        ProfileId = profileId;
        JobTitle = jobTitle;
        Title = title;
        Suffix = suffix;
        About = about;
        Summary = summary;
        Overview = overview;
        Address = address;
    }

    public static ProfileHeader CreateInstance(IGuidGenerator guidGenerator, IClock clock, ProfileId profileId, string title, string suffix,
        string jobTitle, string about,
        string summary, string overview, string address)
    {
        var id = new ProfileHeaderId(guidGenerator.NewGuid());
        return new ProfileHeader(id, profileId, clock, title, suffix, jobTitle, about, summary, overview, address);
    }

    public void UpdateGeneralInfo(
        string title,
        string suffix,
        string jobTitle,
        string about,
        string summary,
        string overview,
        string address)
    {
        Guard.HasSizeLessThanOrEqualTo(jobTitle, MaxJobTitleLength);

        Title = title;
        Suffix = suffix;
        JobTitle = jobTitle;
        About = about;
        Summary = summary;
        Overview = overview;
        Address = address;
    }

    public void AddContactInfo(
        IGuidGenerator guidGenerator,
        IClock clock,
        string value,
        ContactType contactType)
    {
        Guard.IsNotNullOrEmpty(value);
        Guard.IsNotNull(contactType);

        var contactInfo = Entities.ContactInfo.CreateInstance(guidGenerator, clock, Id, value, contactType);
        _contactInfo?.Add(contactInfo);
    }
}