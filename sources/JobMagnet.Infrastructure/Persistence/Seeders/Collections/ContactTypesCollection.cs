using System.Collections.Immutable;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Core.Entities.Contact;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

public record ContactTypesCollection
{
    private readonly List<(string Name, string Class, List<string> Aliases)> _values =
    [
        ("Email", "bx bx-envelope", ["Correo Electrónico", "E-mail"]),
        ("Mobile Phone", "bx bx-mobile", ["Phone", "Teléfonos", "Teléfono Móvil", "Celular", "Móvil"]),
        ("Home Phone", "bx bx-phone", ["Teléfono Fijo", "Teléfono de Casa", "Teléfono Casa"]),
        ("Work Phone", "bx bx-phone-call", [ "Teléfono Trabajo", "Teléfono Oficina", "Teléfono de Trabajo", "Teléfono de Oficina"]),
        ("Website", "bx bx-globe", ["Web Site", "Web-site", "Sitio Web", "Página Web", "Blog", "Portafolio"]),
        ("LinkedIn", "bx bxl-linkedin", []),
        ("GitHub", "bx bxl-github", []),
        ("Twitter", "bx bxl-twitter", ["X"]),
        ("Facebook", "bx bxl-facebook", []),
        ("Instagram", "bx bxl-instagram", []),
        ("YouTube", "bx bxl-youtube", []),
        ("WhatsApp", "bx bxl-whatsapp", ["Wasap", "Whatsapp"]),
        ("Telegram", "bx bxl-telegram", []),
        ("Snapchat", "bx bxl-snapchat", []),
        ("Pinterest", "bx bxl-pinterest", []),
        ("Skype", "bx bxl-skype", []),
        ("Discord", "bx bxl-discord", []),
        ("Twitch", "bx bxl-twitch", []),
        ("TikTok", "bx bxl-tiktok", []),
        ("Reddit", "bx bxl-reddit", []),
        ("Vimeo", "bx bxl-vimeo", [])
    ];

    public int Count => _values.Count;

    public ImmutableList<ContactType> GetContactTypesWithAliases()
    {
        var contactTypes = new List<ContactType>();

        foreach (var (name, iconClass, aliases) in _values)
        {
            var contactType = new ContactType(name, 0, iconClass);

            foreach (var alias in aliases)
            {
                contactType.AddAlias(alias);
            }

            contactTypes.Add(contactType);
        }

        return contactTypes.ToImmutableList();
    }
}