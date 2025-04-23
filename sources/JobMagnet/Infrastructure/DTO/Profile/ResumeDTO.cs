namespace JobMagnet.Infrastructure.DTO.Profile;

// ReSharper disable once ClassNeverInstantiated.Global
public class ResumeDTO
{
    public string JobTitle { get; set; }
    public string About { get; set; }
    public string Summary { get; set; }
    public string Overview { get; set; }
    public string? Title { get; set; }
    public string? Suffix { get; set; }
    public string? Address { get; set; }

    public virtual ProfileDTO Profile { get; set; }
    public virtual ICollection<ContactInfoDTO> ContactInfo { get; set; }
}