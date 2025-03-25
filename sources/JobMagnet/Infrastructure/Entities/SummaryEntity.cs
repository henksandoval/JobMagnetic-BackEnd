using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class SummaryEntity : SoftDeletableEntity<long>
{
    public string Introduction { get; set; }
    public virtual ICollection<EducationEntity> Education { get; set; } = new HashSet<EducationEntity>();
    public virtual ICollection<WorkExperienceEntity> WorkExperiences { get; set; } = new HashSet<WorkExperienceEntity>();
}