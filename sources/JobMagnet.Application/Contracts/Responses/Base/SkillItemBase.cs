namespace JobMagnet.Application.Contracts.Responses.Base;

public sealed record SkillItemBase
{
    public long Id { get; init; }
    public int ProficiencyLevel { get; init; }
    public int Rank { get; init; }
    public string? Name { get; init; }
    public string? Category { get; init; }
    public string? IconUrl { get; init; }
}