using System.ComponentModel.DataAnnotations.Schema;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class ResumeEntity : SoftDeletableEntity<long>
{
    public string JobTitle { get; set; }
    public string About { get; set; }
    public string Summary { get; set; }
    public string Overview { get; set; }
    public string? Title { get; set; }
    public string? Suffix { get; set; }
    public string? Address { get; set; }

    [ForeignKey(nameof(Profile))] public long ProfileId { get; set; }

    public virtual ProfileEntity Profile { get; set; }
    public virtual ICollection<ContactInfoEntity>? ContactInfo { get; set; }
}