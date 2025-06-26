using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Shared.Base;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct HeadlineId(Guid Value) : IStronglyTypedId<Guid>;

public class Headline : SoftDeletableEntity<HeadlineId>
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

    private Headline() : base(new HeadlineId(), Guid.Empty)
    {
    }

    public Headline(
        string title,
        string suffix,
        string jobTitle,
        string about,
        string summary,
        string overview,
        string address,
        HeadlineId id,
        ProfileId profileId) : base(id, Guid.Empty)
    {
        Guard.HasSizeLessThanOrEqualTo(jobTitle!, MaxJobTitleLength);

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

    public void AddContactInfo(string value,
        ContactType contactType)
    {
        Guard.IsNotNullOrEmpty(value);
        Guard.IsNotNull(contactType);

        var contactInfo = new ContactInfo(value, contactType, new ContactInfoId(), Guid.Empty, Id);
        _contactInfo?.Add(contactInfo);
    }
}