namespace JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

public interface IParsedSkill
{
    string? Overview { get; }
    IReadOnlyCollection<IParsedSkillDetail> SkillDetails { get; }
}