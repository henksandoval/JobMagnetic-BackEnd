using System.ComponentModel.DataAnnotations.Schema;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class ContactInfoEntity : SoftDeletableEntity<long>
{
    public required string Value { get; set; }

    [ForeignKey(nameof(ContactType))] public int ContactTypeId { get; set; }
    [ForeignKey(nameof(Resume))] public long ResumeId { get; set; }

    public virtual ContactTypeEntity ContactType { get; set; }
    public virtual ResumeEntity Resume { get; set; }
}