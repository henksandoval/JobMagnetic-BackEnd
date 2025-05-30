using JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class SummaryParseDto : IParsedSummary
{
    public string? Introduction { get; set; }
    public IEnumerable<EducationParseDto> Education { get; set; }
    public IEnumerable<WorkExperienceParseDto> WorkExperiences { get; set; }

    IReadOnlyCollection<IParsedEducation> IParsedSummary.Education =>
        new List<IParsedEducation>(Education).AsReadOnly();
    IReadOnlyCollection<IParsedWorkExperience> IParsedSummary.WorkExperiences =>
        new List<IParsedWorkExperience>(WorkExperiences).AsReadOnly();
}