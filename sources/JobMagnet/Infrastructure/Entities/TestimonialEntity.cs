using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class TestimonialEntity : SoftDeletableEntity<long>
{
    public required string Name { get; set; }
    public required string JobTitle { get; set; }
    public string? PhotoUrl { get; set; }
    public required string Feedback { get; set; }
}