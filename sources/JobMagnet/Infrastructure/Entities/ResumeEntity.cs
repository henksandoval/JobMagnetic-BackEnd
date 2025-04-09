using System.ComponentModel.DataAnnotations.Schema;
using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class ResumeEntity : SoftDeletableEntity<long>
{
    public string JobTitle { get; set; }
    public string About { get; set; }
    public string Summary { get; set; }
    public string Overview { get; set; }
    public string? Title { get; set; }
    public string? Suffix { get; set; }

    [ForeignKey(nameof(Profile))] public long ProfileId { get; set; }

    public virtual ProfileEntity Profile { get; set; }
}