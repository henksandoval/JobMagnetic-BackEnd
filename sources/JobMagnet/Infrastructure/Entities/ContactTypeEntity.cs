using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.Entities;

public class ContactTypeEntity : SoftDeletableEntity<int>
{
    public required string Name { get; set; }
    public string? IconClass { get; set; }
    public string? IconUrl { get; set; }

    public virtual ICollection<ContactInfoEntity> ContactDetails { get; set; }
}