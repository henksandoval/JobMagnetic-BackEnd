using JobMagnet.Shared.Data;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

public static class ContactTypeSeeder
{
    public static int Count => ContactRawData.Data.Count;
    public static SeedingData SeedData { get; } = GenerateSeedData();

    private static SeedingData GenerateSeedData()
    {
        var seedTimestamp = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var types = new List<ContactTypeSeedData>();
        var aliases = new List<ContactAliasSeedData>();

        var typeIdCounter = 1;
        var aliasIdCounter = 1;

        foreach (var rawDef in ContactRawData.Data)
        {
            var typeId = typeIdCounter++;

            types.Add(new ContactTypeSeedData(
                typeId,
                rawDef.Name,
                rawDef.IconClass,
                null,
                seedTimestamp
            ));

            aliases.AddRange(rawDef.Aliases.Select(aliasName =>
                new ContactAliasSeedData(
                    aliasIdCounter++,
                    aliasName,
                    typeId,
                    seedTimestamp
                ))
            );
        }

        return new SeedingData(types, aliases);
    }

    public record ContactTypeSeedData(int Id, string Name, string? IconClass, Uri? IconUrl, DateTime AddedAt, bool IsDeleted = false);

    public record ContactAliasSeedData(int Id, string Alias, int ContactTypeId, DateTime AddedAt, bool IsDeleted = false);

    public record SeedingData(
        IReadOnlyCollection<ContactTypeSeedData> Types,
        IReadOnlyCollection<ContactAliasSeedData> Aliases);
}