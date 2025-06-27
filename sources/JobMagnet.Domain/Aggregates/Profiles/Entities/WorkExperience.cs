
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Shared.Base;
using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct WorkExperienceId(Guid Value) : IStronglyTypedId<WorkExperienceId>;

public class WorkExperience : SoftDeletableAggregate<WorkExperienceId>
{
    private readonly HashSet<WorkHighlight> _highlights = [];

    public string JobTitle { get; private set; }
    public string CompanyName { get; private set; }
    public string CompanyLocation { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string Description { get; private set; }
    public HeadlineId HeadlineId { get; private set; }

    public virtual IReadOnlyCollection<WorkHighlight> Highlights => _highlights;

    private WorkExperience(WorkExperienceId id, DateTimeOffset addedAt, DateTimeOffset? lastModifiedAt, DateTimeOffset? deletedAt) :
        base(id, addedAt, lastModifiedAt, deletedAt)
    {
    }

    private WorkExperience(
        WorkExperienceId id,
        HeadlineId headlineId,
        IClock clock,
        string jobTitle,
        string companyName,
        string companyLocation,
        DateTime startDate,
        DateTime? endDate,
        string description) : base(id, clock)
    {
        Guard.IsNotNullOrWhiteSpace(jobTitle);
        Guard.IsNotNullOrWhiteSpace(companyName);
        Guard.IsNotNullOrWhiteSpace(companyLocation);
        Guard.IsNotNullOrWhiteSpace(description);
        Guard.IsNotNull(startDate);

        if (endDate.HasValue)
            Guard.IsGreaterThan(endDate.Value, startDate);

        Id = id;
        HeadlineId = headlineId;
        JobTitle = jobTitle;
        CompanyName = companyName;
        CompanyLocation = companyLocation;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
    }

    public static WorkExperience CreateInstance(IGuidGenerator guidGenerator, IClock clock, HeadlineId headlineId, string jobTitle, string companyName, string companyLocation, DateTime startDate, DateTime? endDate, string description)
    {
        var id = new WorkExperienceId(guidGenerator.NewGuid());
        return new WorkExperience(id, headlineId, clock,jobTitle, companyName, companyLocation, startDate, endDate, description);
    }

    public void AddResponsibility(WorkHighlight highlight)
    {
        Guard.IsNotNull(highlight);

        _highlights.Add(highlight);
    }
}