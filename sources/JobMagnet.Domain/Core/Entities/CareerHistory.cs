using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class CareerHistory : SoftDeletableEntity<long>
{
    private readonly HashSet<Qualification> _qualifications = [];
    private readonly HashSet<WorkExperience> _workExperiences = [];

    public string Introduction { get; private set; }
    public long ProfileId { get; private set; }
    public virtual IReadOnlyCollection<Qualification> Qualifications => _qualifications;
    public virtual IReadOnlyCollection<WorkExperience> WorkExperiences => _workExperiences;

    private CareerHistory()
    {
    }

    [SetsRequiredMembers]
    public CareerHistory(string introduction, long profileId = 0, long id = 0)
    {
        Guard.IsGreaterThanOrEqualTo(id, 0);
        Guard.IsGreaterThanOrEqualTo(profileId, 0);
        Guard.IsNotNullOrWhiteSpace(introduction);

        Id = id;
        ProfileId = profileId;
        Introduction = introduction;
    }

    public void AddEducation(Qualification education)
    {
        Guard.IsNotNull(education);

        _qualifications.Add(education);
    }

    public void AddWorkExperience(WorkExperience workExperience)
    {
        Guard.IsNotNull(workExperience);

        _workExperiences.Add(workExperience);
    }
}