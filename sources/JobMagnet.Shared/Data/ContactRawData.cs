namespace JobMagnet.Shared.Data;

public record RawContactDefinition(string Name, string IconClass, string[] Aliases);

public static class ContactRawData
{
    public static readonly IReadOnlyList<RawContactDefinition> Data =
    [
        new("Email", "bx bx-envelope", ["Correo Electrónico", "E-mail"]),
        new("Mobile Phone", "bx bx-mobile", ["Phone", "Teléfonos", "Teléfono Móvil", "Celular", "Móvil"]),
        new("Home Phone", "bx bx-phone", ["Teléfono Fijo", "Teléfono de Casa", "Teléfono Casa"]),
        new("Work Phone", "bx bx-phone-call", ["Teléfono Trabajo", "Teléfono Oficina", "Teléfono de Trabajo", "Teléfono de Oficina"]),
        new("Website", "bx bx-globe", ["Web Site", "Web-site", "Sitio Web", "Página Web", "Blog", "Portafolio"]),
        new("LinkedIn", "bx bxl-linkedin", []),
        new("GitHub", "bx bxl-github", []),
        new("Twitter", "bx bxl-twitter", ["X"]),
        new("Facebook", "bx bxl-facebook", []),
        new("Instagram", "bx bxl-instagram", []),
        new("YouTube", "bx bxl-youtube", []),
        new("WhatsApp", "bx bxl-whatsapp", ["Wasap"]),
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
}