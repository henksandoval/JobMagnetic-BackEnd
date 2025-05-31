using System.ComponentModel.DataAnnotations.Schema;
using JobMagnet.Domain.Entities.Base;

namespace JobMagnet.Domain.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class TestimonialEntity : SoftDeletableEntity<long>
{
    public required string Name { get; set; }
    public required string JobTitle { get; set; }
    public string? PhotoUrl { get; set; }
    public required string Feedback { get; set; }

    [ForeignKey(nameof(Profile))] public long ProfileId { get; set; }

    public virtual ProfileEntity Profile { get; set; }
}