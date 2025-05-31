using JobMagnet.Domain.Core.Services.CvParser.Interfaces;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class SkillDetailParseDto : IParsedSkillDetail
{
    public string? Name { get; set; }
    public ushort? Level { get; set; }
}