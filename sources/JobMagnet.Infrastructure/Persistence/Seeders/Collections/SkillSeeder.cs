using JobMagnet.Domain.Aggregates.Skills.Entities;
using JobMagnet.Shared.Data;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

public static class SkillSeeder
{
    public static SeedingData SeedData { get; } = GenerateSeedData();

    private static SeedingData GenerateSeedData()
    {
        var clock = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var guidGenerator = new SeederSequentialGuidGenerator();
        var systemUserId = Guid.Empty;

        var categoryNameToIdMap = new Dictionary<string, Guid>();
        var categories = new List<CategorySeedData>();

        var distinctCategoryNames = SkillRawData.Data
            .Select(r => r.CategoryName)
            .Distinct()
            .ToList();

        if (!distinctCategoryNames.Contains(SkillCategory.DefaultCategoryName))
        {
            distinctCategoryNames.Add(SkillCategory.DefaultCategoryName);
        }

        foreach (var categoryName in distinctCategoryNames)
        {
            var categoryId = guidGenerator.NewGuid();
            categoryNameToIdMap.Add(categoryName, categoryId);
            categories.Add(new CategorySeedData(
                Id: categoryId,
                Name: categoryName,
                AddedAt: clock
            ));
        }

        var types = new List<TypeSeedData>();
        var aliases = new List<AliasSeedData>();

        foreach (var rawDef in SkillRawData.Data)
        {
            var typeId = guidGenerator.NewGuid();
            var categoryId = categoryNameToIdMap[rawDef.CategoryName];

            types.Add(new TypeSeedData(
                Id: typeId,
                Name: rawDef.Name,
                IconUrl: new Uri(rawDef.Uri),
                CategoryId: categoryId,
                AddedAt: clock
            ));

            aliases.AddRange(rawDef.Aliases.Select(aliasName =>
                new AliasSeedData(
                    Id: guidGenerator.NewGuid(),
                    Alias: aliasName,
                    SkillTypeId: typeId,
                    AddedAt: clock
                ))
            );
        }

        return new SeedingData(categories, types, aliases);
    }

    public record CategorySeedData(Guid Id, string Name, DateTimeOffset AddedAt, bool IsDeleted = false);

    public record TypeSeedData(
        Guid Id,
        string Name,
        Uri IconUrl,
        Guid CategoryId,
        DateTimeOffset AddedAt,
        bool IsDeleted = false);

    public record AliasSeedData(Guid Id, string Alias, Guid SkillTypeId, DateTimeOffset AddedAt, bool IsDeleted = false);

    public record SeedingData(
        IReadOnlyCollection<CategorySeedData> Categories,
        IReadOnlyCollection<TypeSeedData> Types,
        IReadOnlyCollection<AliasSeedData> Aliases);
}