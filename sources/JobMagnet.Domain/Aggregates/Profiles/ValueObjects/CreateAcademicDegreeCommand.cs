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

    public record AcademicInfo
    {
        public string Degree { get; init; }
        public string InstitutionName { get; init; }
        public string InstitutionLocation { get; init; }
        public string Description { get; init; }

        public AcademicInfo(
            string degree,
            string institutionName,
            string institutionLocation,
            string description,
            bool applyValidations = true)
        {
            if (applyValidations)
            {
                ValidateField(degree, nameof(degree));
                ValidateField(institutionName, nameof(institutionName));
                ValidateField(institutionLocation, nameof(institutionLocation));
                ValidateField(description, nameof(description));
            }

            Degree = degree?.Trim() ?? string.Empty;
            InstitutionName = institutionName?.Trim() ?? string.Empty;
            InstitutionLocation = institutionLocation?.Trim() ?? string.Empty;
            Description = description?.Trim() ?? string.Empty;
        }

        private static void ValidateField(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{fieldName} cannot be null, empty, or whitespace.", fieldName);
            }
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