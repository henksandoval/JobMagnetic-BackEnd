using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;

namespace JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

public record WorkHighlight
{
    public const int MaxDescriptionLength = 500;
    public string Description { get; private set; } = null!;

    private WorkHighlight()
    {
    }

    internal WorkHighlight(string description)
    {
        Guard.IsNotNullOrWhiteSpace(description);
        Guard.IsLessThanOrEqualTo(description.Length, MaxDescriptionLength);

        Description = description;
    }
}