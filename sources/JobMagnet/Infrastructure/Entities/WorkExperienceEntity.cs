using System.ComponentModel.DataAnnotations.Schema;
using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class WorkExperienceEntity : SoftDeletableEntity<long>
{
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public string CompanyLocation { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; }
    public ICollection<string> Responsibilities { get; set; } = new List<string>();

    [ForeignKey(nameof(Summary))]
    public long SummaryId { get; set; }
    public virtual SummaryEntity Summary { get; set; }
}