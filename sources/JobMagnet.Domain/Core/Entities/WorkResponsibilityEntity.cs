using System.Diagnostics.CodeAnalysis;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class WorkResponsibilityEntity : SoftDeletableEntity<long>
{
    public const int MaxDescriptionLength = 500;
    public long WorkExperienceId { get; private set; }
    public string Description { get; private set; } = null!;

    public virtual WorkExperienceEntity WorkExperience { get; init; } = null!;

    public WorkResponsibilityEntity() { }

    [SetsRequiredMembers]
    public WorkResponsibilityEntity(long workExperienceId, string description)
    {
        WorkExperienceId = workExperienceId;
        Description = description;
    }
}