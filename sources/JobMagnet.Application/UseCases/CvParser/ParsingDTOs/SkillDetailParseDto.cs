using JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class SkillDetailParseDto : IParsedSkillDetail
{
    public string? Name { get; set; }
    public string? Level { get; set; }
}