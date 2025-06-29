using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Shared.Base;
using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct QualificationId(Guid Value) : IStronglyTypedId<QualificationId>;

public class Qualification : SoftDeletableAggregate<QualificationId>
{
    public string Degree { get; private set; }
    public string InstitutionName { get; private set; }
    public string InstitutionLocation { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string Description { get; private set; }
    public CareerHistoryId CareerHistoryId { get; private set; }

    private Qualification() : base() { }

    private Qualification(QualificationId id, CareerHistoryId careerHistoryId, string degree, string institutionName, string institutionLocation,
        DateTime startDate, DateTime? endDate, string description, IClock clock) : base(id, clock)
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

    public static Qualification CreateInstance(IGuidGenerator guidGenerator, IClock clock, CareerHistoryId careerHistoryId, string degree,
        string institutionName, string institutionLocation, DateTime startDate, DateTime? endDate, string description)
    {
        var id = new QualificationId(guidGenerator.NewGuid());
        return new Qualification(id, careerHistoryId, degree, institutionName, institutionLocation, startDate, endDate, description, clock);
    }
}