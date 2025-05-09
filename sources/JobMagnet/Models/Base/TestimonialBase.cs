namespace JobMagnet.Models.Base;

public sealed record TestimonialBase
{
    public long ProfileId { get; init; }
    public string? Name { get; init; }
    public string? JobTitle { get; init; }
    public string? PhotoUrl { get; init; }
    public string? Feedback { get; init; }
}