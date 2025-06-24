namespace JobMagnet.Application.Contracts.Responses.Base;

public sealed record ProjectBase
{
    public long ProfileId { get; init; }
    public int Position { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string? UrlLink { get; init; }
    public string? UrlImage { get; init; }
    public string? UrlVideo { get; init; }
    public string? Type { get; init; }
}