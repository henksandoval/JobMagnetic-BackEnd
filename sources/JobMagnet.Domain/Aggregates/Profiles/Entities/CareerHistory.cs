using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Shared.Base;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct CareerHistoryId(Guid Value) : IStronglyTypedId<Guid>;

public class CareerHistory : SoftDeletableEntity<CareerHistoryId>
{
    private readonly HashSet<Qualification> _qualifications = [];
    private readonly HashSet<WorkExperience> _workExperiences = [];

    public string Introduction { get; private set; }
    public ProfileId ProfileId { get; private set; }
    public virtual IReadOnlyCollection<Qualification> Qualifications => _qualifications;
    public virtual IReadOnlyCollection<WorkExperience> WorkExperiences => _workExperiences;

    private CareerHistory() : base(new CareerHistoryId(), Guid.Empty)
    {
    }

    public CareerHistory(CareerHistoryId id, Guid addedBy, string introduction, ProfileId profileId) : base(id, addedBy)
    {
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