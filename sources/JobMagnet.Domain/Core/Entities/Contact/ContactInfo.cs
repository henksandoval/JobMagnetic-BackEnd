using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities.Contact;

public class ContactInfo : SoftDeletableEntity<long>
{
    public string Value { get; private set; }
    public int ContactTypeId { get; private set; }
    public long ResumeId { get; private set; }
    public virtual ContactType ContactType { get; private set; }

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