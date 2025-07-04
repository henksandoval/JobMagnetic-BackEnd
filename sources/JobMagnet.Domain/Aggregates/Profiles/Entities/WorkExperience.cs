using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Shared.Base.Entities;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public class WorkExperience : SoftDeletableEntity<WorkExperienceId>
{
    private readonly HashSet<WorkHighlight> _highlights = [];

    public string JobTitle { get; private set; }
    public string CompanyName { get; private set; }
    public string CompanyLocation { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string Description { get; private set; }
    public CareerHistoryId CareerHistoryId { get; private set; }

    public virtual IReadOnlyCollection<WorkHighlight> Highlights => _highlights;

    private WorkExperience() : base() { }

    private WorkExperience(
        WorkExperienceId id,
        CareerHistoryId careerHistoryId,
        string jobTitle,
        string companyName,
        string companyLocation,
        DateTime startDate,
        DateTime? endDate,
        string description) : base(id)
    {
        Guard.IsNotNullOrWhiteSpace(jobTitle);
        Guard.IsNotNullOrWhiteSpace(companyName);
        Guard.IsNotNullOrWhiteSpace(companyLocation);
        Guard.IsNotNullOrWhiteSpace(description);
        Guard.IsNotNull(startDate);

        if (endDate.HasValue)
            Guard.IsGreaterThan(endDate.Value, startDate);

        Id = id;
        CareerHistoryId = careerHistoryId;
        JobTitle = jobTitle;
        CompanyName = companyName;
        CompanyLocation = companyLocation;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
    }

    public static WorkExperience CreateInstance(IGuidGenerator guidGenerator, CareerHistoryId careerHistoryId, string jobTitle,
        string companyName, string companyLocation, DateTime startDate, DateTime? endDate, string description)
    {
        var id = new WorkExperienceId(guidGenerator.NewGuid());
        return new WorkExperience(id, careerHistoryId, jobTitle, companyName, companyLocation, startDate, endDate, description);
    }

    public void AddResponsibility(WorkHighlight highlight)
    {
        Guard.IsNotNull(highlight);

        _highlights.Add(highlight);
    }
}