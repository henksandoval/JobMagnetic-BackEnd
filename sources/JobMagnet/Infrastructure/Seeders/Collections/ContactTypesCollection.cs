using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Infrastructure.Seeders.Collections;

public record ContactTypesCollection
{
    public readonly IReadOnlyList<ContactTypeEntity> ContactTypes =
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
}