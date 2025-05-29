using JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

namespace JobMagnet.Application.UseCases.CvParser.ParsingDTOs;

public class SkillParseDto : IParsedSkill
{
    public string? Overview { get; set; }
    public IEnumerable<SkillDetailParseDto> SkillDetailList { private get; set; }

    IReadOnlyCollection<IParsedSkillDetail> IParsedSkill.SkillDetails =>
        new List<IParsedSkillDetail>(SkillDetailList).AsReadOnly();
}