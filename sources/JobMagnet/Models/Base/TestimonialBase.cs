namespace JobMagnet.Models.Base;

public record TestimonialBase
{
    public long ProfileId { get; set; }
    public string? Name { get; set; }
    public string? JobTitle { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Feedback { get; set; }
}