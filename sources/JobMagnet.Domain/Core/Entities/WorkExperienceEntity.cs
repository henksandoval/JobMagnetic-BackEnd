using System.ComponentModel.DataAnnotations.Schema;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class WorkExperienceEntity : SoftDeletableEntity<long>
{
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public string CompanyLocation { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; }

    [ForeignKey(nameof(Summary))] public long SummaryId { get; set; }

    public virtual SummaryEntity Summary { get; set; }
    public virtual ICollection<WorkResponsibilityEntity> Responsibilities { get; set; } = new List<WorkResponsibilityEntity>();
}