using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class WorkResponsibilityEntity : SoftDeletableEntity<long>
{
    public const int MaxDescriptionLength = 500;
    public long WorkExperienceId { get; private set; }
    public string Description { get; private set; } = null!;

    private WorkResponsibilityEntity() { }

    [SetsRequiredMembers]
    internal WorkResponsibilityEntity(string description, long workExperienceId = 0, long id = 0)
    {
        Guard.IsGreaterThanOrEqualTo(id, 0);
        Guard.IsGreaterThanOrEqualTo(workExperienceId, 0);
        Guard.IsNotNullOrWhiteSpace(description);
        Guard.IsLessThanOrEqualTo(description.Length, MaxDescriptionLength);

        Id = id;
        WorkExperienceId = workExperienceId;
        Description = description;
    }
}