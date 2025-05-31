using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class ContactTypeEntity : SoftDeletableEntity<int>
{
    public string Name { get; set; }
    public string? IconClass { get; set; }
    public string? IconUrl { get; set; }

    public virtual ICollection<ContactInfoEntity> ContactDetails { get; set; }
}