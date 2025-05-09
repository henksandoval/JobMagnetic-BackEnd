namespace JobMagnet.Models.Base;

public sealed record SkillItemBase
{
    public long Id { get; set; }
    public int ProficiencyLevel { get; set; }
    public int Rank { get; set; }
    public string? Name { get; set; }
    public string? Category { get; set; }
    public string? IconUrl { get; set; }
}