using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities.Skills;

public class SkillCategory : SoftDeletableEntity<ushort>
{
    private const int MinNameLength = 2;
    public const int MaxNameLength = 50;
    public const string DefaultCategoryName = "General";
    public const ushort DefaultCategoryId = 1;

    public string Name { get; private set; }

    private SkillCategory()
    {
    }

    [SetsRequiredMembers]
    public SkillCategory(string name, ushort id = 0)
    {
        Guard.IsNotNullOrWhiteSpace(name);
        Guard.HasSizeGreaterThan(name, MinNameLength);
        Guard.HasSizeLessThan(name, MaxNameLength);

        Id = id;
        Name = name;
    }
}