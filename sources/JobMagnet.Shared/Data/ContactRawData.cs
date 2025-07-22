namespace JobMagnet.Shared.Data;

public record RawContactDefinition(int Id, string Name, string IconClass, RawSimpleDefinition[] Aliases);

public static class ContactRawData
{
    public static readonly IReadOnlyList<RawContactDefinition> Data =
    [
        new(301, "Email", "bx bx-envelope",
        [
            new RawSimpleDefinition(401, "Correo Electrónico"),
            new RawSimpleDefinition(402, "E-mail")
        ]),
        new(302, "Mobile Phone", "bx bx-mobile",
        [
            new RawSimpleDefinition(403, "Phone"),
            new RawSimpleDefinition(404, "Teléfonos"),
            new RawSimpleDefinition(405, "Teléfono Móvil"),
            new RawSimpleDefinition(406, "Celular"),
            new RawSimpleDefinition(407, "Móvil")
        ]),
        new(303, "Home Phone", "bx bx-phone",
        [
            new RawSimpleDefinition(408, "Teléfono Fijo"),
            new RawSimpleDefinition(409, "Teléfono de Casa"),
            new RawSimpleDefinition(410, "Teléfono Casa")
        ]),
        new(304, "Work Phone", "bx bx-phone-call",
        [
            new RawSimpleDefinition(411, "Teléfono Trabajo"),
            new RawSimpleDefinition(412, "Teléfono Oficina"),
            new RawSimpleDefinition(413, "Teléfono de Trabajo"),
            new RawSimpleDefinition(414, "Teléfono de Oficina")
        ]),
        new(305, "Website", "bx bx-globe",
        [
            new RawSimpleDefinition(415, "Web Site"),
            new RawSimpleDefinition(416, "Web-site"),
            new RawSimpleDefinition(417, "Sitio Web"),
            new RawSimpleDefinition(418, "Página Web"),
            new RawSimpleDefinition(419, "Blog"),
            new RawSimpleDefinition(420, "Portafolio")
        ]),
        new(306, "LinkedIn", "bx bxl-linkedin", []),
        new(307, "GitHub", "bx bxl-github", []),
        new(308, "Twitter", "bx bxl-twitter",
        [
            new RawSimpleDefinition(421, "X")
        ]),
        new(309, "Facebook", "bx bxl-facebook", []),
        new(310, "Instagram", "bx bxl-instagram", []),
        new(311, "YouTube", "bx bxl-youtube", []),
        new(312, "WhatsApp", "bx bxl-whatsapp",
        [
            new RawSimpleDefinition(422, "Wasap")
        ]),
        new(313, "Telegram", "bx bxl-telegram", []),
        new(314, "Snapchat", "bx bxl-snapchat", []),
        new(315, "Pinterest", "bx bxl-pinterest", []),
        new(316, "Skype", "bx bxl-skype", []),
        new(317, "Discord", "bx bxl-discord", []),
        new(318, "Twitch", "bx bxl-twitch", []),
        new(319, "TikTok", "bx bxl-tiktok", []),
        new(320, "Reddit", "bx bxl-reddit", []),
        new(321, "Vimeo", "bx bxl-vimeo", [])
    ];
}