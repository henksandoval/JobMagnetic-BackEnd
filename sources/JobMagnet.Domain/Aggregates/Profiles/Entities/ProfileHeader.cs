using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Shared.Base.Entities;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public class ProfileHeader : SoftDeletableEntity<ProfileHeaderId>
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
        string title,
        string suffix,
        string jobTitle,
        string about,
        string summary,
        string overview,
        string address) : base(id)
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

    public static ProfileHeader CreateInstance(IGuidGenerator guidGenerator, ProfileId profileId, string title, string suffix,
        string jobTitle, string about,
        string summary, string overview, string address)
    {
        var id = new ProfileHeaderId(guidGenerator.NewGuid());
        return new ProfileHeader(id, profileId, title, suffix, jobTitle, about, summary, overview, address);
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

        var contactInfo = Entities.ContactInfo.CreateInstance(guidGenerator, Id, value, contactType);
        _contactInfo?.Add(contactInfo);
    }
}