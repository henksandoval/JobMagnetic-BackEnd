using JobMagnet.Application.Settings;
using JobMagnet.Domain.Core.Services.CvParser.Interfaces;
using Newtonsoft.Json;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class EducationParseDto : IParsedEducation
{
    public string? Degree { get; set; }
    public string? InstitutionName { get; set; }
    public string? InstitutionLocation { get; set; }
    public string? Description { get; set; }

    [JsonConverter(typeof(FlexibleDateOnlyConverter))]
    public DateOnly? StartDate { get; set; }

    [JsonConverter(typeof(FlexibleDateOnlyConverter))]
    public DateOnly? EndDate { get; set; }
}