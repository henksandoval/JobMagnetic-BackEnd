using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Shared.Base.Aggregates;
using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct CareerHistoryId(Guid Value) : IStronglyTypedId<CareerHistoryId>;

public class CareerHistory : SoftDeletableAggregateRoot<CareerHistoryId>
{
    private readonly HashSet<Qualification> _qualifications = [];
    private readonly HashSet<WorkExperience> _workExperiences = [];

    public string Introduction { get; private set; }
    public ProfileId ProfileId { get; private set; }
    public virtual IReadOnlyCollection<Qualification> Qualifications => _qualifications;
    public virtual IReadOnlyCollection<WorkExperience> WorkExperiences => _workExperiences;

    private CareerHistory() : base() { }

    private CareerHistory(CareerHistoryId id, IClock clock, string introduction, ProfileId profileId) : base(id, clock)
    {
        Guard.IsNotNullOrWhiteSpace(introduction);

        Id = id;
        ProfileId = profileId;
        Introduction = introduction;
    }

    public static CareerHistory CreateInstance(IGuidGenerator guidGenerator, IClock clock, ProfileId profileId, string introduction)
    {
        var id = new CareerHistoryId(guidGenerator.NewGuid());
        return new CareerHistory(id, clock, introduction, profileId);
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