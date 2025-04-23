namespace JobMagnet.Infrastructure.DTO.Profile;

public class ContactInfoDTO
{
    public required string Value { get; set; }

    public virtual ContactTypeDTO ContactType { get; set; }
    public virtual ResumeDTO Resume { get; set; }
}