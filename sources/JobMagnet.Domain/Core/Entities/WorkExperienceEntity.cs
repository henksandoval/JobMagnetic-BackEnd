using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class WorkExperienceEntity : SoftDeletableEntity<long>
{
    private readonly HashSet<WorkResponsibilityEntity> _responsibilities = [];

    public string JobTitle { get; private set; }
    public string CompanyName { get; private set; }
    public string CompanyLocation { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string Description { get; private set; }
    public long SummaryId { get; private set; }

    public virtual IReadOnlyCollection<WorkResponsibilityEntity> Responsibilities => _responsibilities;

    private WorkExperienceEntity()
    {
    }

    [SetsRequiredMembers]
    public WorkExperienceEntity(string jobTitle,
        string companyName,
        string companyLocation,
        DateTime startDate,
        DateTime? endDate,
        string description,
        long summaryId = 0,
        long id = 0)
    {
        Guard.IsGreaterThanOrEqualTo(id, 0);
        Guard.IsGreaterThanOrEqualTo(summaryId, 0);
        Guard.IsNotNullOrWhiteSpace(jobTitle);
        Guard.IsNotNullOrWhiteSpace(companyName);
        Guard.IsNotNullOrWhiteSpace(companyLocation);
        Guard.IsNotNullOrWhiteSpace(description);
        Guard.IsNotNull(startDate);

        if (endDate.HasValue)
            Guard.IsGreaterThan(endDate.Value, startDate);

        Id = id;
        SummaryId = summaryId;
        JobTitle = jobTitle;
        CompanyName = companyName;
        CompanyLocation = companyLocation;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
    }

    public void AddResponsibility(WorkResponsibilityEntity responsibility)
    {
        Guard.IsNotNull(responsibility);

        _responsibilities.Add(responsibility);
    }
}