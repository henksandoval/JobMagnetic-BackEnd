using System.Collections.Immutable;
using JobMagnet.Domain.Entities;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record ServiceCollection
{
    private readonly long _serviceId;

    private readonly IList<GalleryProperties> _values =
    [
        new("Web Development",
            "Building responsive and user-friendly websites.",
            "https://example.com/web-development",
            "https://cdn.pixabay.com/photo/2024/08/06/10/43/wine-8949009_1280.jpg",
            "image"),
        new("UI/UX Design",
            "Creating intuitive and engaging user interfaces.",
            "https://example.com/ui-ux-design",
            "https://cdn.pixabay.com/photo/2023/08/11/08/29/highland-cattle-8183107_1280.jpg",
            "image",
            "https://example.com/video2.mp4"),
        new("Web and Brand Graphic Design",
            "Creating intuitive and engaging user interfaces.",
            "https://example.com/ux-design",
            "https://cdn.pixabay.com/photo/2024/02/20/13/21/mountains-8585535_1280.jpg",
            "image",
            "https://example.com/video3.mp4"),
        new("SEO Consulting",
            "Creating intuitive and engaging user interfaces.",
            "https://example.com/ui-ux-design2",
            "https://cdn.pixabay.com/photo/2024/01/25/10/50/mosque-8531576_1280.jpg",
            "image")
    ];

    public ServiceCollection(long serviceId = 0)
    {
        _serviceId = serviceId;
    }

    public IReadOnlyList<ServiceGalleryItemEntity> GetServicesGallery()
    {
        return _values.Select(x => new ServiceGalleryItemEntity
        {
            Id = 0,
            ServiceId = _serviceId,
            Title = x.Title,
            Description = x.Description,
            UrlLink = x.UrlLink,
            UrlImage = x.UrlImage,
            Type = x.Type,
            UrlVideo = x.UrlVideo,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        }).ToImmutableList();
    }
}