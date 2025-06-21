using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Core.Entities.Contact;

namespace JobMagnet.Domain.Core.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class ResumeEntity : SoftDeletableEntity<long>
{
    public const int MaxTitleLength = 100;

    private readonly HashSet<ContactInfo> _contactInfo = [];

    public string? Title { get; set; }
    public string? JobTitle { get; set; }
    public string? About { get; set; }
    public string? Summary { get; set; }
    public string? Overview { get; set; }
    public string? Suffix { get; set; }
    public string? Address { get; set; }

    [ForeignKey(nameof(Profile))] public long ProfileId { get; set; }

    public virtual ProfileEntity Profile { get; set; }
    public virtual IReadOnlyCollection<ContactInfo>? ContactInfo => _contactInfo;

    public void AddContactInfo(string value, ContactType contactType)
    {
        Guard.IsNotNullOrEmpty(value);
        Guard.IsNotNull(contactType);

        var contactInfo = new ContactInfo(0, value, contactType, Id);
        _contactInfo?.Add(contactInfo);
    }
}