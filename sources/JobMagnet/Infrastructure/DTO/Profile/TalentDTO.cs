namespace JobMagnet.Infrastructure.DTO.Profile;

public class TalentDTO
{
    public required string Description { get; set; }

    public virtual ProfileDTO Profile { get; set; }
}