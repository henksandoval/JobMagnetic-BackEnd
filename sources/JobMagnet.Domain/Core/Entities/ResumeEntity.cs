using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Core.Entities.ContactInfo;

namespace JobMagnet.Domain.Core.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class ResumeEntity : SoftDeletableEntity<long>
{
    private readonly HashSet<ContactInfoEntity> _contactInfo = [];

    public string? JobTitle { get; set; }
    public string? About { get; set; }
    public string? Summary { get; set; }
    public string? Overview { get; set; }
    public string? Title { get; set; }
    public string? Suffix { get; set; }
    public string? Address { get; set; }

    [ForeignKey(nameof(Profile))] public long ProfileId { get; set; }

    public virtual ProfileEntity Profile { get; set; }
    public virtual IReadOnlyCollection<ContactInfoEntity>? ContactInfo => _contactInfo;

    public void AddContactInfo(string value, ContactTypeEntity contactType)
    {
        Guard.IsNotNullOrEmpty(value);
        Guard.IsNotNull(contactType);

        var contactInfo = new ContactInfoEntity(0, value, this, contactType);
        _contactInfo?.Add(contactInfo);
    }
}