using System.Globalization;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

public record CreateAcademicDegreeCommand
{
    public IGuidGenerator GuidGenerator { get; init; }
    public CareerHistoryId CareerHistoryId { get; init; }
    public AcademicInfo Academic { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }

    public record AcademicInfo(string Degree, string InstitutionName, string InstitutionLocation, string Description, bool ApplyValidations = true)
    {
        public string Degree { get; init; } = ApplyValidations ? ValidateAcademicInfo(Degree, InstitutionName, InstitutionLocation, Description) : Degree?.Trim() ?? string.Empty;
        
        private static string ValidateAcademicInfo(string degree, string institutionName, string institutionLocation, string description)
        {
            if (string.IsNullOrWhiteSpace(degree))
                throw new ArgumentException("Degree cannot be null or empty.", nameof(degree));
            if (string.IsNullOrWhiteSpace(institutionName))
                throw new ArgumentException("Institution name cannot be null or empty.", nameof(institutionName));
            if (string.IsNullOrWhiteSpace(institutionLocation))
                throw new ArgumentException("Institution location cannot be null or empty.", nameof(institutionLocation));
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be null or empty.", nameof(description));

            return degree.Trim();
        }
    }
    public CreateAcademicDegreeCommand(
        IGuidGenerator guidGenerator,
        CareerHistoryId careerHistoryId,
        AcademicInfo academic,
        DateTime startDate,
        DateTime? endDate)
    {
        GuidGenerator = guidGenerator;
        CareerHistoryId = careerHistoryId;
        Academic = academic;
        StartDate = startDate;
        EndDate = endDate;
    }
    
    public static CreateAcademicDegreeCommand FromStrings(
        IGuidGenerator guidGenerator,
        CareerHistoryId careerHistoryId,
        AcademicInfo academic,
        string startDateString,
        string? endDateString,
        IFormatProvider formatProvider = null)
    {
        formatProvider ??= CultureInfo.InvariantCulture;
    
        var startDate = DateTime.Parse(startDateString, formatProvider);
        var endDate = string.IsNullOrWhiteSpace(endDateString) 
            ? (DateTime?)null 
            : DateTime.Parse(endDateString, formatProvider);
    
        return new CreateAcademicDegreeCommand(
            guidGenerator,
            careerHistoryId,
            academic,
            startDate,
            endDate);
    }
}