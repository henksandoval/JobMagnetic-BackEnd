using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Shared.Base;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct QualificationId(Guid Value) : IStronglyTypedId<Guid>;

public class Qualification : SoftDeletableEntity<QualificationId>
{
    public string Degree { get; private set; }
    public string InstitutionName { get; private set; }
    public string InstitutionLocation { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string Description { get; private set; }
    public HeadlineId HeadlineId { get; private set; }

    [SetsRequiredMembers]
    public Qualification(string degree, string institutionName, string institutionLocation, DateTime startDate, DateTime? endDate,
        string description, HeadlineId headlineId, QualificationId id, Guid addedBy) : base(id, addedBy)
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
        HeadlineId = headlineId;
        Description = description;
        Id = id;
    }
}