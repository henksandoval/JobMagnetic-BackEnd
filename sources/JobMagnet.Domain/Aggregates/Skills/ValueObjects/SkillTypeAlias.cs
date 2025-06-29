using CommunityToolkit.Diagnostics;

namespace JobMagnet.Domain.Aggregates.Skills.ValueObjects;

public record SkillTypeAlias
{
    public const int MaxAliasLength = 50;

    public string Alias { get; private set; }

    internal SkillTypeAlias(string alias)
    {
        Guard.IsNotNullOrWhiteSpace(alias);
        Guard.HasSizeLessThanOrEqualTo(alias, MaxAliasLength);

        Alias = alias;
    }
}