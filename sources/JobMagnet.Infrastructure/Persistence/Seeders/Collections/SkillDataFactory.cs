using System.Collections.Immutable;
using JobMagnet.Domain.Aggregates.Skills.Entities;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

public static class SkillDataFactory
{
    private static readonly IReadOnlyList<RawSkillDefinition> RawData =
    [
        new("HTML", "https://cdn.simpleicons.org/html5", "Software Development", []),
        new("CSS", "https://cdn.simpleicons.org/css3", "Software Development", []),
        new("JavaScript", "https://cdn.simpleicons.org/javascript", "Software Development", ["JS"]),
        new("C#", "https://cdn.simpleicons.org/dotnet", "Software Development", []),
        new("TypeScript", "https://cdn.simpleicons.org/typescript", "Software Development", ["TS"]),
        new("Angular", "https://cdn.simpleicons.org/angular", "Software Development", []),
        new("PostgreSQL", "https://cdn.simpleicons.org/postgresql", "Software Development", ["Postgres"]),
        new("React", "https://cdn.simpleicons.org/react", "Software Development", []),
        new("Bootstrap", "https://cdn.simpleicons.org/bootstrap", "Software Development", []),
        new("Vue", "https://cdn.simpleicons.org/vuedotjs", "Software Development", ["Vue.js"]),
        new("Git", "https://cdn.simpleicons.org/git", "Software Development", []),
        new("Blazor", "https://cdn.simpleicons.org/blazor", "Software Development", []),
        new("RabbitMQ", "https://cdn.simpleicons.org/rabbitmq", "Software Development", ["Rabbit MQ"]),
        new("Docker", "https://cdn.simpleicons.org/docker", "Software Development", [])
    ];

    public static SeedingData SeedData { get; } = GenerateSeedData();

    public static int Count => RawData.Count;

    private static SeedingData GenerateSeedData()
    {
        var seedTimestamp = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var systemUserId = Guid.Empty;

        var categories = new Dictionary<string, SkillCategory>
        {
            { SkillCategory.DefaultCategoryName, new SkillCategory(new SkillCategoryId(), SkillCategory.DefaultCategoryName) }
        };
        var types = new List<TypeSeedData>();
        var aliases = new List<AliasSeedData>();

        var categoryIdCounter = categories.Count;
        var typeIdCounter = 1;
        var aliasIdCounter = 1;

        foreach (var rawDef in RawData)
        {
            if (!categories.TryGetValue(rawDef.CategoryName, out var category))
            {
                categoryIdCounter++;
                categories.Add(rawDef.CategoryName, new SkillCategory(new SkillCategoryId(), rawDef.CategoryName));
            }

            var typeId = typeIdCounter++;
            types.Add(new TypeSeedData(typeId, rawDef.Name, new Uri(rawDef.Uri), (ushort)categoryIdCounter, seedTimestamp, systemUserId));

            aliases.AddRange(rawDef.Aliases.Select(aliasName =>
                new AliasSeedData(aliasIdCounter++, aliasName, typeId, seedTimestamp, systemUserId))
            );
        }

        return new SeedingData(categories.Values, types, aliases);
    }

    public static ImmutableList<SkillType> GetDomainSkillTypes()
    {
        var skills = new List<SkillType>();
        var categoryCache = new Dictionary<string, SkillCategory>();

        foreach (var rawDef in RawData)
        {
            if (!categoryCache.TryGetValue(rawDef.CategoryName, out var category))
            {
                category = new SkillCategory(new SkillCategoryId(), rawDef.CategoryName);
                categoryCache.Add(rawDef.CategoryName, category);
            }

            var skill = new SkillType(new SkillTypeId(),rawDef.Name, category, new Uri(rawDef.Uri));

            foreach (var alias in rawDef.Aliases) skill.AddAlias(alias);

            skills.Add(skill);
        }

        return skills.ToImmutableList();
    }

    private record RawSkillDefinition(string Name, string Uri, string CategoryName, string[] Aliases);

    public record CategorySeedData(ushort Id, string Name, DateTime AddedAt, Guid AddedBy, bool IsDeleted = false);

    public record TypeSeedData(
        int Id,
        string Name,
        Uri IconUrl,
        ushort CategoryId,
        DateTime AddedAt,
        Guid AddedBy,
        bool IsDeleted = false);

    public record AliasSeedData(int Id, string Alias, int SkillTypeId, DateTime AddedAt, Guid AddedBy, bool IsDeleted = false);

    public record SeedingData(
        IReadOnlyCollection<SkillCategory> Categories,
        IReadOnlyCollection<TypeSeedData> Types,
        IReadOnlyCollection<AliasSeedData> Aliases);
}