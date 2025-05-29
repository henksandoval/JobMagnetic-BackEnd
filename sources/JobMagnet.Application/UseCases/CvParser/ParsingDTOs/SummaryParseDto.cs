using JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class SummaryParseDto : IParsedSummary
{
    public string? Introduction { get; set; }
    public IEnumerable<EducationParseDto> EducationList { private get; set; }
    public IEnumerable<WorkExperienceParseDto> WorkExperienceList { private get; set; }

    public IReadOnlyCollection<IParsedEducation> Education =>
        new List<IParsedEducation>(EducationList).AsReadOnly();
    public IReadOnlyCollection<IParsedWorkExperience> WorkExperiences =>
        new List<IParsedWorkExperience>(WorkExperienceList).AsReadOnly();
}