using System.Collections.Immutable;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Infrastructure.Seeders.Collections;

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

    private readonly IReadOnlyList<ContactTypeEntity> _contactTypes =
    [
        new()
        {
            Id = 0,
            Name = "Email",
            IconClass = "bx bx-envelope",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Mobile Phone",
            IconClass = "bx bx-mobile",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Home Phone",
            IconClass = "bx bx-phone",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Work Phone",
            IconClass = "bx bx-phone-call",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Website",
            IconClass = "bx bx-globe",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "LinkedIn",
            IconClass = "bx bxl-linkedin",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "GitHub",
            IconClass = "bx bxl-github",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Twitter",
            IconClass = "bx bxl-twitter",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Facebook",
            IconClass = "bx bxl-facebook",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Instagram",
            IconClass = "bx bxl-instagram",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "YouTube",
            IconClass = "bx bxl-youtube",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "WhatsApp",
            IconClass = "bx bxl-whatsapp",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Telegram",
            IconClass = "bx bxl-telegram",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Snapchat",
            IconClass = "bx bxl-snapchat",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Pinterest",
            IconClass = "bx bxl-pinterest",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Skype",
            IconClass = "bx bxl-skype",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Discord",
            IconClass = "bx bxl-discord",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Twitch",
            IconClass = "bx bxl-twitch",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "TikTok",
            IconClass = "bx bxl-tiktok",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Reddit",
            IconClass = "bx bxl-reddit",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Vimeo",
            IconClass = "bx bxl-vimeo",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        }
    ];

    public ImmutableList<ContactTypeEntity> GetContactTypes()
    {
        return _values.Select(x => CreateContactType(x.Name, x.Class)).ToImmutableList();
    }

    private static ContactTypeEntity CreateContactType(string name, string iconClass)
    {
        return new ContactTypeEntity
        {
            Id = 0,
            Name = name,
            IconClass = iconClass,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        };
    }
}