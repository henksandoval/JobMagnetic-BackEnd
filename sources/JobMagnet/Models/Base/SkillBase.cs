using JobMagnet.Models.Commands.Skill;

namespace JobMagnet.Models.Base;

public sealed record SkillBase
{
    public required long ProfileId { get; set; }
    public required string? Overview { get; set; }
    public required IList<SkillItemCommand> SkillDetails { get; set; }
}