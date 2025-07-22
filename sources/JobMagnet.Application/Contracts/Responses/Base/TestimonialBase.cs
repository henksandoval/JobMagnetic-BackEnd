namespace JobMagnet.Application.Contracts.Responses.Base;

public sealed record TestimonialBase
{
    public Guid ProfileId { get; init; }
    public string? Name { get; init; }
    public string? JobTitle { get; init; }
    public string? PhotoUrl { get; init; }
    public string? Feedback { get; init; }
}