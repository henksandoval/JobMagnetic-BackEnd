namespace JobMagnet.Application.Contracts.Responses.Base;

public sealed record SkillBase
{
    public long ProfileId { get; init; }
    public string? Overview { get; init; }
    public IList<SkillItemBase> Skills { get; init; } = Enumerable.Empty<SkillItemBase>().ToList();
}