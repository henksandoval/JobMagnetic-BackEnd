using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.DTO.Profile;

public class ContactTypeDTO
{
    public required string Name { get; set; }
    public string? IconClass { get; set; }
    public string? IconUrl { get; set; }

    public virtual ICollection<ContactInfoDTO> ContactDetails { get; set; }
}