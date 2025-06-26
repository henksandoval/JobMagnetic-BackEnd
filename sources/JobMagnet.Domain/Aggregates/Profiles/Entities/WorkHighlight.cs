using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public class WorkHighlight
{
    public const int MaxDescriptionLength = 500;
    public long WorkExperienceId { get; private set; }
    public string Description { get; private set; } = null!;

    private WorkHighlight()
    {
    }

    [SetsRequiredMembers]
    internal WorkHighlight(string description, long workExperienceId = 0)
    {
        Guard.IsGreaterThanOrEqualTo(workExperienceId, 0);
        Guard.IsNotNullOrWhiteSpace(description);
        Guard.IsLessThanOrEqualTo(description.Length, MaxDescriptionLength);

        WorkExperienceId = workExperienceId;
        Description = description;
    }
}