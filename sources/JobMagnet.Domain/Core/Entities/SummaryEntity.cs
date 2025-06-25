using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class SummaryEntity : SoftDeletableEntity<long>
{
    private readonly HashSet<EducationEntity> _educationHistory = [];
    private readonly HashSet<WorkExperienceEntity> _workExperiences = [];

    public string Introduction { get; private set; }
    public long ProfileId { get; private set; }
    public virtual IReadOnlyCollection<EducationEntity> Education => _educationHistory;
    public virtual IReadOnlyCollection<WorkExperienceEntity> WorkExperiences => _workExperiences;

    private SummaryEntity()
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

    public void AddEducation(EducationEntity education)
    {
        Guard.IsNotNull(education);

        _educationHistory.Add(education);
    }

    public void AddWorkExperience(WorkExperienceEntity workExperience)
    {
        Guard.IsNotNull(workExperience);

        _workExperiences.Add(workExperience);
    }
}