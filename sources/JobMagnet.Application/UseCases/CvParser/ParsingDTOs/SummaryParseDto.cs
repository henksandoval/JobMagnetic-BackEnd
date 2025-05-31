using JobMagnet.Domain.Core.Services.CvParser.Interfaces;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class SummaryParseDto : IParsedSummary
{
    public IEnumerable<EducationParseDto> Education { get; set; }
    public IEnumerable<WorkExperienceParseDto> WorkExperiences { get; set; }
    public string? Introduction { get; set; }

    IReadOnlyCollection<IParsedEducation> IParsedSummary.Education =>
        new List<IParsedEducation>(Education).AsReadOnly();

    IReadOnlyCollection<IParsedWorkExperience> IParsedSummary.WorkExperiences =>
        new List<IParsedWorkExperience>(WorkExperiences).AsReadOnly();
}