using JobMagnet.Domain.Domain.Services.CvParser.Interfaces;
using Newtonsoft.Json;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class EducationParseDto : IParsedEducation
{
    public string? SchoolName { get; set; }
    public string? Degree { get; set; }
    public string? FieldOfStudy { get; set; }

    [JsonConverter(typeof(FlexibleDateOnlyConverter))]
    public DateOnly? StartDate { get; set; }

    [JsonConverter(typeof(FlexibleDateOnlyConverter))]
    public DateOnly? EndDate { get; set; }
}