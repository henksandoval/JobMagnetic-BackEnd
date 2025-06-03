namespace JobMagnet.Application.Contracts.Queries.Profile;

public record ProfileQueryParameters
{
    public string? ProfileSlug { get; init; }
}