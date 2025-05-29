using JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class ContactInfoParseDto : IParsedContactInfo
{
    public string? Type { get; set; }
    public string? Value { get; set; }
}