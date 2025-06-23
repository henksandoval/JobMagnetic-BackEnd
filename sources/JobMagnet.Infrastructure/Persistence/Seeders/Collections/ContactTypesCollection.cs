using System.Collections.Immutable;
using JobMagnet.Domain.Core.Entities.Contact;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

public static class ContactTypeDataFactory
{
    private record RawContactDefinition(string Name, string IconClass, string[] Aliases);

    private static readonly IReadOnlyList<RawContactDefinition> RawData =
    [
        new("Email", "bx bx-envelope", ["Correo Electrónico", "E-mail"]),
        new("Mobile Phone", "bx bx-mobile", ["Phone", "Teléfonos", "Teléfono Móvil", "Celular", "Móvil"]),
        new("Home Phone", "bx bx-phone", ["Teléfono Fijo", "Teléfono de Casa", "Teléfono Casa"]),
        new("Work Phone", "bx bx-phone-call", [ "Teléfono Trabajo", "Teléfono Oficina", "Teléfono de Trabajo", "Teléfono de Oficina"]),
        new("Website", "bx bx-globe", ["Web Site", "Web-site", "Sitio Web", "Página Web", "Blog", "Portafolio"]),
        new("LinkedIn", "bx bxl-linkedin", []),
        new("GitHub", "bx bxl-github", []),
        new("Twitter", "bx bxl-twitter", ["X"]),
        new("Facebook", "bx bxl-facebook", []),
        new("Instagram", "bx bxl-instagram", []),
        new("YouTube", "bx bxl-youtube", []),
        new("WhatsApp", "bx bxl-whatsapp", ["Wasap", "Whatsapp"]),
        new("Telegram", "bx bxl-telegram", []),
        new("Snapchat", "bx bxl-snapchat", []),
        new("Pinterest", "bx bxl-pinterest", []),
        new("Skype", "bx bxl-skype", []),
        new("Discord", "bx bxl-discord", []),
        new("Twitch", "bx bxl-twitch", []),
        new("TikTok", "bx bxl-tiktok", []),
        new("Reddit", "bx bxl-reddit", []),
        new("Vimeo", "bx bxl-vimeo", [])
    ];

    public record ContactTypeSeedData(int Id, string Name, string? IconClass, Uri? IconUrl, DateTime AddedAt, Guid AddedBy, bool IsDeleted = false);
    public record ContactAliasSeedData(int Id, string Alias, int ContactTypeId, DateTime AddedAt, Guid AddedBy, bool IsDeleted = false);

    public record SeedingData(
        IReadOnlyCollection<ContactTypeSeedData> Types,
        IReadOnlyCollection<ContactAliasSeedData> Aliases);

    public static SeedingData SeedData { get; } = GenerateSeedData();
    public static int Count => RawData.Count;

    private static SeedingData GenerateSeedData()
    {
        var seedTimestamp = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var systemUserId = Guid.Empty;

        var types = new List<ContactTypeSeedData>();
        var aliases = new List<ContactAliasSeedData>();

        var typeIdCounter = 1;
        var aliasIdCounter = 1;

        foreach (var rawDef in RawData)
        {
            var typeId = typeIdCounter++;

            types.Add(new ContactTypeSeedData(
                typeId,
                rawDef.Name,
                rawDef.IconClass,
                null,
                seedTimestamp,
                systemUserId
            ));

            aliases.AddRange(rawDef.Aliases.Select(aliasName =>
                new ContactAliasSeedData(
                    aliasIdCounter++,
                    aliasName,
                    typeId,
                    seedTimestamp,
                    systemUserId
                ))
            );
        }

        return new SeedingData(types, aliases);
    }

    public static ImmutableList<ContactType> GetDomainContactTypes()
    {
        var contactTypes = new List<ContactType>();

        foreach (var rawDef in RawData)
        {
            var contactType = new ContactType(rawDef.Name, 0, rawDef.IconClass);

            foreach (var alias in rawDef.Aliases)
            {
                contactType.AddAlias(alias);
            }

            contactTypes.Add(contactType);
        }

        return contactTypes.ToImmutableList();
    }
}