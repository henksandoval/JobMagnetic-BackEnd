using System.Collections.Immutable;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record ProjectCollection
{
    private readonly long _profileId;

    private readonly IList<GalleryProperties> _values =
    [
        new("Aventuras Animales",
            "Cada fotografía captura momentos únicos y comportamientos fascinantes.",
            "https://waylet.es/",
            "https://images.pexels.com/photos/617278/pexels-photo-617278.jpeg",
            "CAT"),
        new("Horizontes Naturales",
            "Cada imagen captura la esencia de lugares únicos, desde montañas imponentes hasta costas tranquilas, invitándote a explorar la belleza del mundo",
            "https://biati-digital.github.io/glightbox/",
            "https://th.bing.com/th/id/OIP.iwFhHHKPOqAJUDO-iSov_wHaE8?rs=1&pid=ImgDetMain",
            "NATURE"),
        new("Movil Truck",
            "Plataforma de transporte inteligente; solución tecnológica diseñada para abordar de manera eficiente el transporte de mercancías por carretera.",
            "https://moviltruck.com/",
            "https://moviltruck.com/wp-content/uploads/2023/11/Hero-1-.png",
            "WebPage"),
        new("Red And Blue Parrot",
            "Hermosos y encantadores Guacamayas en ambiente natural.",
            "https://www.pexels.com/es-es/buscar/guacamayo/",
            "https://images.pexels.com/photos/1427447/pexels-photo-1427447.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
            "NATURE",
            "https://videos.pexels.com/video-files/17325162/17325162-uhd_1440_2560_30fps.mp4"),
        new("Aventuras Animales",
            "Cada fotografía captura momentos únicos y comportamientos fascinantes.",
            "https://www.pexels.com/es-es/buscar/animales/",
            "https://images.pexels.com/photos/60023/baboons-monkey-mammal-freeze-60023.jpeg?auto=compress&cs=tinysrgb&w=600",
            "Monkey"),
        new("Cats",
            "Cada fotografía captura momentos únicos y comportamientos fascinantes.",
            "https://www.pexels.com/es-es/buscar/gatos/",
            "https://images.pexels.com/photos/416160/pexels-photo-416160.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
            "CAT"),
        new("Dogs",
            "Cada fotografía captura momentos únicos y comportamientos fascinantes.",
            "https://www.pexels.com/es-es/buscar/pastor%20alemán%20cachorro/",
            "https://images.pexels.com/photos/19949287/pexels-photo-19949287/free-photo-of-animal-perro-mascota-mono.jpeg?auto=compress&cs=tinysrgb&w=1200",
            "DOG")
    ];

    public ProjectCollection(long profileId = 0)
    {
        _profileId = profileId;
    }

    public ImmutableList<Project> GetProjects()
    {
        return _values.Select((x, index) => new Project(
                x.Title,
                x.Description,
                x.UrlLink,
                x.UrlImage,
                x.UrlVideo ?? string.Empty,
                x.Type,
                ++index,
                _profileId))
            .ToImmutableList();
    }
}