using JobMagnet.Domain.Entities.Base;

namespace JobMagnet.Domain.Entities;

public class ContactTypeEntity : SoftDeletableEntity<int>
{
    public string Name { get; set; }
    public string? IconClass { get; set; }
    public string? IconUrl { get; set; }

    public virtual ICollection<ContactInfoEntity> ContactDetails { get; set; }
}