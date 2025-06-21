using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities.Skills;

public class SkillTypeAlias : SoftDeletableEntity<int>
{
    public const int MaxAliasLength = 50;

    public string Alias { get; private set; }
    public int SkillTypeId { get; private set; }
    public bool SkillTypeExist => Id > 0 && !IsDeleted;

    private SkillTypeAlias() {}

    [SetsRequiredMembers]
    internal SkillTypeAlias(string alias, int skillTypeId)
    {
        Guard.IsNotNullOrWhiteSpace(alias);
        Guard.IsGreaterThanOrEqualTo<int>(skillTypeId, 0);

        Alias = alias;
        SkillTypeId = skillTypeId;
    }
}