using System.Collections.Immutable;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

public record ContactTypesCollection
{
    private readonly List<(string Name, string Class)> _values =
    [
        ("Email", "bx bx-envelope"),
        ("Mobile Phone", "bx bx-mobile"),
        ("Home Phone", "bx bx-phone"),
        ("Work Phone", "bx bx-phone-call"),
        ("Website", "bx bx-globe"),
        ("LinkedIn", "bx bxl-linkedin"),
        ("GitHub", "bx bxl-github"),
        ("Twitter", "bx bxl-twitter"),
        ("Facebook", "bx bxl-facebook"),
        ("Instagram", "bx bxl-instagram"),
        ("YouTube", "bx bxl-youtube"),
        ("WhatsApp", "bx bxl-whatsapp"),
        ("Telegram", "bx bxl-telegram"),
        ("Snapchat", "bx bxl-snapchat"),
        ("Pinterest", "bx bxl-pinterest"),
        ("Skype", "bx bxl-skype"),
        ("Discord", "bx bxl-discord"),
        ("Twitch", "bx bxl-twitch"),
        ("TikTok", "bx bxl-tiktok"),
        ("Reddit", "bx bxl-reddit"),
        ("Vimeo", "bx bxl-vimeo")
    ];

    public ImmutableList<ContactTypeEntity> GetContactTypes()
    {
        return _values.Select(x => CreateContactType(x.Name, x.Class)).ToImmutableList();
    }

    private static ContactTypeEntity CreateContactType(string name, string iconClass)
    {
        return new ContactTypeEntity(0, name, iconClass, null);
    }
}