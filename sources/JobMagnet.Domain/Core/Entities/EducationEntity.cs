using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class EducationEntity : SoftDeletableEntity<long>
{
    public string Degree { get; private set; }
    public string InstitutionName { get; private set; }
    public string InstitutionLocation { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string Description { get; private set; }
    public long SummaryId { get; private set; }

    [SetsRequiredMembers]
    public EducationEntity(string degree, string institutionName, string institutionLocation, DateTime startDate, DateTime? endDate,
        string description, long summaryId = 0, long id = 0)
    {
        Guard.IsGreaterThanOrEqualTo(id, 0);
        Guard.IsGreaterThanOrEqualTo(summaryId, 0);
        Guard.IsNotNullOrWhiteSpace(degree);
        Guard.IsNotNullOrWhiteSpace(institutionName);
        Guard.IsNotNullOrWhiteSpace(institutionLocation);
        Guard.IsNotNullOrWhiteSpace(description);
        Guard.IsNotNull(startDate);

        if (DateTime.TryParse(endDate?.ToString(), out var end))
            Guard.IsGreaterThan(end, startDate);

        Degree = degree;
        InstitutionName = institutionName;
        InstitutionLocation = institutionLocation;
        StartDate = startDate;
        EndDate = endDate;
        SummaryId = summaryId;
        Description = description;
        Id = id;
    }
}