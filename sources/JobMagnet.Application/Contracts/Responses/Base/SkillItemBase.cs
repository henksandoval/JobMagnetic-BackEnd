namespace JobMagnet.Application.Contracts.Responses.Base;

public sealed record SkillItemBase
{
    public long Id { get; init; }
    public ushort ProficiencyLevel { get; init; }
    public ushort Rank { get; init; }
    public string? Name { get; init; }
    public string? Category { get; init; }
    public string? IconUrl { get; init; }
}