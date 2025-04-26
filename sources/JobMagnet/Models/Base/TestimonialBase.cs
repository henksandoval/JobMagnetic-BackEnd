namespace JobMagnet.Models.Base;

public abstract class TestimonialBase
{
    public required long ProfileId { get; set; }
    public required string Name { get; set; }
    public required string JobTitle { get; set; }
    public string? PhotoUrl { get; set; }
    public required string Feedback { get; set; }
}