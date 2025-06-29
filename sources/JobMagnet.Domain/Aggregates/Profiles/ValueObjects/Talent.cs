using CommunityToolkit.Diagnostics;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public record Talent
{
    public const int MaxNameLength = 50;
    public string Description { get; private set; }

    private Talent() { }

    public Talent(string description)
    {
        Guard.IsNotNullOrWhiteSpace(description);
        Guard.IsLessThanOrEqualTo(description.Length, MaxNameLength);

        Description = description;
    }
}