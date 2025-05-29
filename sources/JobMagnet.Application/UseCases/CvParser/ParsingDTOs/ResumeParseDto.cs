using JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class ResumeParseDto : IParsedResume
{
    public string? JobTitle { get; set; }
    public string? About { get; set; }
    public string? Summary { get; set; }
    public string? Overview { get; set; }
    public string? Title { get; set; }
    public string? Suffix { get; set; }
    public string? Address { get; set; }
    public IEnumerable<ContactInfoParseDto> ContactInfoList { private get; set; }

    public IReadOnlyCollection<IParsedContactInfo> ContactInfo =>
        new List<IParsedContactInfo>(ContactInfoList).AsReadOnly();
}