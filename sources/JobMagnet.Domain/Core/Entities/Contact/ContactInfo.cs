using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities.Contact;

public class ContactInfo : SoftDeletableEntity<long>
{
    public string Value { get; set; }
    public int ContactTypeId { get; set; }
    public long ResumeId { get; set; }

    public virtual ContactType ContactType { get; set; }

    private ContactInfo() { }

    [SetsRequiredMembers]
    internal ContactInfo(long id, string value, ContactType contactType, long resumeId)
    {
        Guard.IsGreaterThanOrEqualTo(id, 0);
        Guard.IsGreaterThanOrEqualTo(resumeId, 0);
        Guard.IsNotNullOrEmpty(value);
        Guard.IsNotNull(contactType);

        Id = id;
        Value = value;
        ContactTypeId = contactType.Id;
        ContactType = contactType;
        ResumeId = resumeId;
    }
}