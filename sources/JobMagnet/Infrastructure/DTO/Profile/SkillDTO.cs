namespace JobMagnet.Infrastructure.DTO.Profile;

// ReSharper disable once ClassNeverInstantiated.Global
public class SkillDTO
{
    public string? Overview { get; set; }
    public virtual ICollection<SkillItemDTO> SkillDetails { get; set; } = new HashSet<SkillItemDTO>();

    public virtual ProfileDTO Profile { get; set; }
}