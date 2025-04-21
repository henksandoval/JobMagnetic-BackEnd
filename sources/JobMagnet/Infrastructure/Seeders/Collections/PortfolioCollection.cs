using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Infrastructure.Seeders.Collections;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record PortfolioCollection(long ProfileId = 0)
{
    public readonly IReadOnlyList<PortfolioGalleryEntity> PortfolioGallery =
    [
        new()
        {
            Id = 0,
            Position = 1,
            Title = "Aventuras Animales",
            Description = "Cada fotografía captura momentos únicos y comportamientos fascinantes.",
            UrlLink = "https://waylet.es/",
            UrlImage = "https://images.pexels.com/photos/617278/pexels-photo-617278.jpeg",
            Type = "CAT",
            UrlVideo = string.Empty,
            ProfileId = ProfileId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Position = 2,
            Title = "Horizontes Naturales",
            Description =
                "Cada imagen captura la esencia de lugares únicos, desde montañas imponentes hasta costas tranquilas, invitándote a explorar la belleza del mundo",
            UrlLink = "https://biati-digital.github.io/glightbox/",
            UrlImage = "https://th.bing.com/th/id/OIP.iwFhHHKPOqAJUDO-iSov_wHaE8?rs=1&pid=ImgDetMain",
            Type = "NATURE",
            UrlVideo = string.Empty,
            ProfileId = ProfileId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Position = 3,
            Title = "Movil Truck",
            Description =
                "Plataforma de transporte inteligente; solución tecnológica diseñada para abordar de manera eficiente el transporte de mercancías por carretera.",
            UrlLink = "https://moviltruck.com/",
            UrlImage = "https://moviltruck.com/wp-content/uploads/2023/11/Hero-1-.png",
            Type = "WebPage",
            UrlVideo = string.Empty,
            ProfileId = ProfileId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Position = 4,
            Title = "Aventuras Animales",
            Description = "Cada fotografía captura momentos únicos y comportamientos fascinantes.",
            UrlLink = string.Empty,
            UrlImage = "https://images.pexels.com/photos/617278/pexels-photo-617278.jpeg",
            Type = "CAT",
            UrlVideo = string.Empty,
            ProfileId = ProfileId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Position = 5,
            Title = "Aventuras Animales",
            Description = "Cada fotografía captura momentos únicos y comportamientos fascinantes.",
            UrlLink = string.Empty,
            UrlImage = "https://images.pexels.com/photos/617278/pexels-photo-617278.jpeg",
            Type = "CAT",
            UrlVideo = string.Empty,
            ProfileId = ProfileId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Position = 6,
            Title = "Music",
            Description =
                "Cada imagen captura la esencia de la musica, el sonido llega al alma dando una hermosa sensacion de relajacion",
            UrlLink = "",
            UrlImage = "https://i0.wp.com/www.nus.agency/wp-content/uploads/2023/03/musica-arte-scaled.jpg?ssl=1",
            Type = "Music",
            UrlVideo = string.Empty,
            ProfileId = ProfileId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Position = 7,
            Title = "Red And Blue Parrot",
            Description = "Hermosos y encantadores Guacamayas en ambiente natural.",
            UrlLink = string.Empty,
            UrlImage =
                "https://images.pexels.com/photos/1427447/pexels-photo-1427447.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
            Type = "NATURE",
            UrlVideo = "https://videos.pexels.com/video-files/17325162/17325162-uhd_1440_2560_30fps.mp4",
            ProfileId = ProfileId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        }
    ];
}