using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Shared.Base;
using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Skills.Entities;

public readonly record struct SkillCategoryId(Guid Value) : IStronglyTypedId<SkillCategoryId>;

public class SkillCategory : SoftDeletableAggregate<SkillCategoryId>
{
    private const int MinNameLength = 2;
    public const int MaxNameLength = 50;
    public const string DefaultCategoryName = "General";
    public static Guid DefaultCategoryId = new("00000001-0000-0000-0000-000000000000");

    public string Name { get; private set; }

    private SkillCategory() : base() {}

    private SkillCategory(SkillCategoryId id, IClock clock, string name) : base(id, clock)
    {
        Guard.IsNotNullOrWhiteSpace(name);
        Guard.HasSizeGreaterThan(name, MinNameLength);
        Guard.HasSizeLessThan(name, MaxNameLength);

        Id = id;
        Name = name;
    }

    public static SkillCategory CreateInstance(IGuidGenerator guidGenerator, IClock clock, string name)
    {
        var id = new SkillCategoryId(guidGenerator.NewGuid());
        return new SkillCategory(id, clock, name);
    }
}