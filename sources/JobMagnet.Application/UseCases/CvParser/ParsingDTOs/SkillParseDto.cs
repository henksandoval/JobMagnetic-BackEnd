using JobMagnet.Domain.Core.Services.CvParser.Interfaces;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class SkillParseDto : IParsedSkill
{
    public IEnumerable<SkillDetailParseDto> SkillDetails { get; set; }
    public string? Overview { get; set; }

    IReadOnlyCollection<IParsedSkillDetail> IParsedSkill.SkillDetails =>
        new List<IParsedSkillDetail>(SkillDetails).AsReadOnly();
}