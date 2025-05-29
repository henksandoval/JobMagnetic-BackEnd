using System.ComponentModel.DataAnnotations.Schema;
using JobMagnet.Domain.Entities.Base;

namespace JobMagnet.Domain.Entities;

public class TalentEntity : SoftDeletableEntity<long>
{
    public required string Description { get; set; }
    [ForeignKey(nameof(Profile))] public long ProfileId { get; set; }
    public virtual ProfileEntity Profile { get; set; }
}