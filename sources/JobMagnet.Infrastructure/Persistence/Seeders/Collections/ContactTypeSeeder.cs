using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Shared.Data;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

public record ContactTypeSeedData(ContactTypeId Id, string Name, string? IconClass, Uri? IconUrl, DateTimeOffset AddedAt, bool IsDeleted = false);

public record ContactAliasSeedData(Guid Id, string Alias, ContactTypeId ContactTypeId);

public static class ContactTypeSeeder
{
    public static int Count => ContactRawData.Data.Count;
    public static SeedingData SeedData { get; } = GenerateSeedData();

    private static SeedingData GenerateSeedData()
    {
        var addedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);

        var types = new List<ContactTypeSeedData>();
        var aliases = new List<ContactAliasSeedData>();

        foreach (var rawDef in ContactRawData.Data)
        {
            var typeId = new ContactTypeId(SeederSequentialGuidGenerator.FromInt(rawDef.id));

            types.Add(new ContactTypeSeedData(
                typeId,
                rawDef.Name,
                rawDef.IconClass,
                null,
                addedAt
            ));

            aliases.AddRange(rawDef.Aliases.Select(alias =>
                new ContactAliasSeedData(
                    SeederSequentialGuidGenerator.FromInt(alias.Id),
                    alias.Name,
                    typeId
                ))
            );
        }

        return new SeedingData(types, aliases);
    }

    public record SeedingData(
        IReadOnlyCollection<ContactTypeSeedData> Types,
        IReadOnlyCollection<ContactAliasSeedData> Aliases);
}