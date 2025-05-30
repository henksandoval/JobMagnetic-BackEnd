using JobMagnet.Domain.Domain.Services.CvParser.Interfaces;
using Newtonsoft.Json;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class WorkExperienceParseDto : IParsedWorkExperience
{
    public string? JobTitle { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyLocation { get; set; }

    [JsonConverter(typeof(FlexibleDateOnlyConverter))]
    public DateOnly? StartDate { get; set; }

    [JsonConverter(typeof(FlexibleDateOnlyConverter))]
    public DateOnly? EndDate { get; set; }
    public string? Description { get; set; }
}