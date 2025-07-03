namespace JobMagnet.Application.Contracts.Responses.Base;

public sealed record SkillBase
{
    public ushort ProficiencyLevel { get; init; }
    public string? Name { get; init; }
}