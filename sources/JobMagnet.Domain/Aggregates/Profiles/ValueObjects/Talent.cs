using CommunityToolkit.Diagnostics;

namespace JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

public record Talent
{
    public const int MaxNameLength = 50;
    public Guid Id { get; private set; }
    public ProfileId ProfileId { get; private set; }
    public string Description { get; private set; }
    
    private Talent() { }

    private Talent(string description)
    {
        Guard.IsNotNullOrWhiteSpace(description);
        Guard.IsLessThanOrEqualTo(description.Length, MaxNameLength);

        Description = description;
    }

    public static Talent CreateInstance(string description)
    {
        return new Talent(description);
    }
}