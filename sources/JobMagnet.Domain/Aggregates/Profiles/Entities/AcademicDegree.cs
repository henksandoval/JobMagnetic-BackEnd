using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Shared.Base.Entities;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public class AcademicDegree : SoftDeletableEntity<AcademicDegreeId>
{
    public string Degree { get; private set; }
    public string InstitutionName { get; private set; }
    public string InstitutionLocation { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string Description { get; private set; }
    public CareerHistoryId CareerHistoryId { get; private set; }

    private AcademicDegree()
    {
    }

    private AcademicDegree(AcademicDegreeId id, CareerHistoryId careerHistoryId, string degree, string institutionName, string institutionLocation,
        DateTime startDate, DateTime? endDate, string description) : base(id)
    {
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
        CareerHistoryId = careerHistoryId;
        Description = description;
    }

    public static AcademicDegree CreateInstance(IGuidGenerator guidGenerator, CareerHistoryId careerHistoryId, string degree,
        string institutionName, string institutionLocation, DateTime startDate, DateTime? endDate, string description)
    {
        var id = new AcademicDegreeId(guidGenerator.NewGuid());
        return new AcademicDegree(id, careerHistoryId, degree, institutionName, institutionLocation, startDate, endDate, description);
    }

    public void Update(string degree, string institutionName, string institutionLocation,
        DateTime startDate, DateTime? endDate, string description)
    {
        Guard.IsNotNullOrWhiteSpace(degree);
        Guard.IsNotNullOrWhiteSpace(institutionName);
        Guard.IsNotNullOrWhiteSpace(institutionLocation);
        Guard.IsNotNullOrWhiteSpace(description);
        Guard.IsNotNull(startDate);

        if (endDate.HasValue)
            Guard.IsGreaterThan(endDate.Value, startDate);

        Degree = degree;
        InstitutionName = institutionName;
        InstitutionLocation = institutionLocation;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
    }
}