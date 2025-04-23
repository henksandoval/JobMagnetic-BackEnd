namespace JobMagnet.Infrastructure.DTO.Profile;

// ReSharper disable once ClassNeverInstantiated.Global
public class SummaryDTO
{
    public string Introduction { get; set; }
    public virtual ICollection<EducationDTO> Education { get; set; } = new HashSet<EducationDTO>();

    public virtual ICollection<WorkExperienceDTO> WorkExperiences { get; set; } =
        new HashSet<WorkExperienceDTO>();

    public virtual ProfileDTO Profile { get; set; }
}