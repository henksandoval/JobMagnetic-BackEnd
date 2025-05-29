namespace JobMagnet.Application.Models.Base;

public sealed record ResumeCommandBase
{
    public long ProfileId { get; init; }
    public string? JobTitle { get; init; }
    public string? About { get; init; }
    public string? Summary { get; init; }
    public string? Overview { get; init; }
    public string? Title { get; init; }
    public string? Suffix { get; init; }
    public string? Address { get; init; }
}