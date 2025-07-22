namespace JobMagnet.Application.Contracts.Responses.Base;

public sealed record SkillSetBase
{
    public Guid ProfileId { get; init; }
    public string? Overview { get; init; }
    public IList<SkillBase> Skills { get; init; } = Enumerable.Empty<SkillBase>().ToList();
}