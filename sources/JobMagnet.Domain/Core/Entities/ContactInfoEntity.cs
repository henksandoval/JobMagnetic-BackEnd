using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class ContactInfoEntity : SoftDeletableEntity<long>
{
    public required string Value { get; set; }

    [ForeignKey(nameof(ContactType))] public int ContactTypeId { get; set; }
    [ForeignKey(nameof(Resume))] public long ResumeId { get; set; }

    public virtual ContactTypeEntity ContactType { get; set; }
    public virtual ResumeEntity Resume { get; set; }

    private ContactInfoEntity() { }

    [SetsRequiredMembers]
    public ContactInfoEntity(long id, string value, ResumeEntity resume, ContactTypeEntity contactType)
    {
        Guard.IsGreaterThanOrEqualTo(0, id);
        Guard.IsNotNullOrEmpty(value);
        Guard.IsNotNull(resume);
        Guard.IsNotNull(contactType);

        Id = id;
        Value = value;
        ContactTypeId = contactType.Id;
        ContactType = contactType;
        ResumeId = resume.Id;
        Resume = resume;
    }
}