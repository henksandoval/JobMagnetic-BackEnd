using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities.ContactInfo;

public class ContactInfoEntity : SoftDeletableEntity<long>
{
    public string Value { get; set; }
    public int ContactTypeId { get; set; }
    public long ResumeId { get; set; }

    public virtual ContactTypeEntity ContactType { get; set; }

    private ContactInfoEntity() { }

    [SetsRequiredMembers]
    internal ContactInfoEntity(long id, string value, ContactTypeEntity contactType, long resumeId)
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