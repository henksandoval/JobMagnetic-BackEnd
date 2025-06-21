using System.Diagnostics.CodeAnalysis;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities.Skills;

public class SkillCategory : SoftDeletableEntity<ushort>
{
    public string Name { get; private set; }

    private SkillCategory() { }

    [SetsRequiredMembers]
    public SkillCategory(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }
}