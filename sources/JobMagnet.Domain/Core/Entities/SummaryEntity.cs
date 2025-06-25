using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class SummaryEntity : SoftDeletableEntity<long>
{
    public string Introduction { get; set; }
    public virtual ICollection<EducationEntity> Education { get; set; } = new HashSet<EducationEntity>();

    public virtual ICollection<WorkExperienceEntity> WorkExperiences { get; set; } =
        new HashSet<WorkExperienceEntity>();

    [ForeignKey(nameof(Profile))] public long ProfileId { get; set; }

    public virtual ProfileEntity Profile { get; set; }

    public SummaryEntity()
    {
    }

    [SetsRequiredMembers]
    public SummaryEntity(string introduction, long profileId = 0, long id = 0)
    {
        Guard.IsGreaterThanOrEqualTo(id, 0);
        Guard.IsGreaterThanOrEqualTo(profileId, 0);
        Guard.IsNotNullOrWhiteSpace(introduction);

        Id = id;
        ProfileId = profileId;
        Introduction = introduction;
    }
}