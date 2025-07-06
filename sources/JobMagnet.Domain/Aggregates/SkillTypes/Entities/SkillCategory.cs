using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Shared.Base.Entities;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.SkillTypes.Entities;

public class SkillCategory : SoftDeletableEntity<SkillCategoryId>
{
    private const int MinNameLength = 2;
    public const int MaxNameLength = 50;
    public const string DefaultCategoryName = "General";
    public static Guid DefaultCategoryId = new("00000001-0000-0000-0000-000000000000");

    public string Name { get; private set; }

    private SkillCategory() : base() {}

    private SkillCategory(SkillCategoryId id, string name) : base(id)
    {
        Guard.IsNotNullOrWhiteSpace(name);
        Guard.HasSizeGreaterThan(name, MinNameLength);
        Guard.HasSizeLessThan(name, MaxNameLength);

        Id = id;
        Name = name;
    }

    public static SkillCategory CreateInstance(IGuidGenerator guidGenerator, string name)
    {
        var id = new SkillCategoryId(guidGenerator.NewGuid());
        return new SkillCategory(id, name);
    }
}