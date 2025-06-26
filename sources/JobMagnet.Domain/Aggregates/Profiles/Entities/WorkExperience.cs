using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Shared.Base;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct WorkExperienceId(Guid Value) : IStronglyTypedId<Guid>;

public class WorkExperience : SoftDeletableEntity<WorkExperienceId>
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

    private WorkExperience() : base(new WorkExperienceId(), Guid.Empty)
    {
    }

    [SetsRequiredMembers]
    public WorkExperience(string jobTitle,
        string companyName,
        string companyLocation,
        DateTime startDate,
        DateTime? endDate,
        string description,
        HeadlineId headlineId,
        WorkExperienceId id) : base(id, Guid.Empty)
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

    public void AddResponsibility(WorkHighlight highlight)
    {
        Guard.IsNotNull(highlight);

        _highlights.Add(highlight);
    }
}