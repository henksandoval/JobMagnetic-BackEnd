namespace JobMagnet.Models.Base;

public sealed record SkillBase
{
    public long ProfileId { get; set; }
    public string? Overview { get; set; }
    public IList<SkillItemBase> SkillDetails { get; set; } = Enumerable.Empty<SkillItemBase>().ToList();
}