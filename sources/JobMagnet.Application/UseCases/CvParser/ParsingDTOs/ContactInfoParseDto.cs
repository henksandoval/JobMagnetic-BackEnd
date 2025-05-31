using JobMagnet.Domain.Core.Services.CvParser.Interfaces;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class ContactInfoParseDto : IParsedContactInfo
{
    public string? ContactType { get; set; }
    public string? Value { get; set; }
}