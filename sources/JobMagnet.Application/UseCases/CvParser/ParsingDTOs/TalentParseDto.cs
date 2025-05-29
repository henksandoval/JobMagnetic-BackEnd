using JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class TalentParseDto : IParsedTalent
{
    public string? Description { get; set; }
    string? IParsedTalent.Description => Description;
}