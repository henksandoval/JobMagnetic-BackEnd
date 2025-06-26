using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Shared.Base;

namespace JobMagnet.Domain.Aggregates.Skills.Entities;

public readonly record struct SkillCategoryId(Guid Value) : IStronglyTypedId<Guid>
{
    public static SkillCategoryId Default => new(Guid.Empty);
}

public class SkillCategory : SoftDeletableEntity<SkillCategoryId>
{
    private const int MinNameLength = 2;
    public const int MaxNameLength = 50;
    public const string DefaultCategoryName = "General";

    public string Name { get; private set; }

    private SkillCategory() : base(new SkillCategoryId(), Guid.Empty)
    {
    }

    public SkillCategory(SkillCategoryId id, string name) : base(id, Guid.Empty)
    {
        Guard.IsNotNullOrWhiteSpace(name);
        Guard.HasSizeGreaterThan(name, MinNameLength);
        Guard.HasSizeLessThan(name, MaxNameLength);

        Id = id;
        Name = name;
    }
}