using System.Diagnostics.CodeAnalysis;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities.Skills;

public class SkillTypeAlias : SoftDeletableEntity<int>
{
    public string Alias { get; private set; }
    public int SkillTypeId { get; private set; }
    public virtual SkillType SkillType { get; private set; }

    public bool SkillTypeExist => Id > 0 && !IsDeleted;

    private SkillTypeAlias() {}

    [SetsRequiredMembers]
    internal SkillTypeAlias(string alias, SkillType skillType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alias, nameof(alias));
        ArgumentNullException.ThrowIfNull(skillType, nameof(skillType));

        Alias = alias;
        SkillType = skillType;
        SkillTypeId = skillType.Id;
    }
}