using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Core.Entities.Contact;

namespace JobMagnet.Domain.Core.Entities;

public class ResumeEntity : SoftDeletableEntity<long>
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
    public long ProfileId { get; private set; }

    public virtual IReadOnlyCollection<ContactInfo>? ContactInfo => _contactInfo;

    private ResumeEntity()
    {
    }

    [SetsRequiredMembers]
    public ResumeEntity(
        string title = "",
        string suffix = "",
        string jobTitle = "",
        string about = "",
        string summary = "",
        string overview = "",
        string address= "",
        long id = 0,
        long profileId = 0)
    {
        Guard.IsGreaterThanOrEqualTo(id, 0);
        Guard.IsGreaterThanOrEqualTo(profileId, 0);
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

        var contactInfo = new ContactInfo(0, value, contactType, Id);
        _contactInfo?.Add(contactInfo);
    }
}