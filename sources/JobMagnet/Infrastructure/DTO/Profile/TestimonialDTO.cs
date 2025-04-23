namespace JobMagnet.Infrastructure.DTO.Profile;

// ReSharper disable once ClassNeverInstantiated.Global
public class TestimonialDTO
{
    public required string Name { get; set; }
    public required string JobTitle { get; set; }
    public string? PhotoUrl { get; set; }
    public required string Feedback { get; set; }

    public virtual ProfileDTO Profile { get; set; }
}