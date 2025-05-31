namespace JobMagnet.Application.Contracts.Queries.Profile;

public record ProfileQueryParameters
{
    public string? Name { get; init; }
}