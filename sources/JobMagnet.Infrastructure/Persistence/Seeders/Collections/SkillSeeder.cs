using JobMagnet.Domain.Aggregates.Skills.Entities;
using JobMagnet.Shared.Data;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

public record CategorySeedData(SkillCategoryId Id, string Name, DateTimeOffset AddedAt, bool IsDeleted = false);

public record TypeSeedData(SkillTypeId Id, string Name, Uri IconUrl, SkillCategoryId CategoryId, DateTimeOffset AddedAt, bool IsDeleted = false);

public record AliasSeedData(Guid Id, string Alias, SkillTypeId SkillTypeId);

public static class SkillSeeder
{
    public record SeedingData(
        IReadOnlyCollection<CategorySeedData> Categories,
        IReadOnlyCollection<TypeSeedData> Types,
        IReadOnlyCollection<AliasSeedData> Aliases);

    public static SeedingData SeedData { get; } =
        GenerateSeedData();

    private static SeedingData GenerateSeedData()
    {
        var addedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);

        var categoryNameToIdMap = new Dictionary<string, SkillCategoryId>();
        var categories = new List<CategorySeedData>();

        var categoryList = Enumerable.Repeat((Id: SkillCategory.DefaultCategoryId, Name: SkillCategory.DefaultCategoryName), 1)
            .Concat(SkillRawData.Data
                .Where(data => !string.Equals(data.Category.Name, SkillCategory.DefaultCategoryName, StringComparison.InvariantCulture))
                .Select(r =>
                {
                    var categoryId = SeederSequentialGuidGenerator.FromInt(r.Category.Id);
                    return (Id: categoryId, r.Category.Name);
                })
                .Distinct()
            );

        foreach (var category in categoryList)
        {
            var skillCategoryId = new SkillCategoryId(category.Id);
            categoryNameToIdMap.Add(category.Name, skillCategoryId);
            categories.Add(new CategorySeedData(
                Id: skillCategoryId,
                Name: category.Name,
                AddedAt: addedAt
            ));
        }

        var types = new List<TypeSeedData>();
        var aliases = new List<AliasSeedData>();

        foreach (var rawSkill in SkillRawData.Data)
        {
            var typeId = new SkillTypeId(SeederSequentialGuidGenerator.FromInt(rawSkill.SkillId));
            var categoryId = categoryNameToIdMap[rawSkill.Category.Name];

            types.Add(new TypeSeedData(
                Id: typeId,
                Name: rawSkill.Name,
                IconUrl: new Uri(rawSkill.Uri),
                CategoryId: categoryId,
                AddedAt: addedAt
            ));

            aliases.AddRange(rawSkill.Aliases.Select(alias =>
                new AliasSeedData(
                    Id: SeederSequentialGuidGenerator.FromInt(alias.Id),
                    Alias: alias.Name,
                    SkillTypeId: typeId
                ))
            );
        }

        return new SeedingData(categories, types, aliases);
    }
}